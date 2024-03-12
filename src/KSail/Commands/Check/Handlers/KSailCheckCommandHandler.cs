using System.Diagnostics;
using k8s;
using k8s.Models;
using KSail.Extensions;

namespace KSail.Commands.Check.Handlers;

class KSailCheckCommandHandler()
{
  readonly HashSet<string> _kustomizations = [];
  readonly HashSet<string> _successfulKustomizations = [];
  readonly Stopwatch _stopwatch = Stopwatch.StartNew();
  readonly Stopwatch _stopwatchTotal = Stopwatch.StartNew();

  internal async Task<int> HandleAsync(string context, CancellationToken token, string? kubeconfig = null)
  {
    Console.WriteLine("👀 Checking the status of the cluster");
    var kubernetesClient = (kubeconfig is not null) switch
    {
      true => new Kubernetes(KubernetesClientConfiguration.BuildConfigFromConfigFile(kubeconfig)),
      false => CreateKubernetesClientFromClusterName(context)
    };
    var responseTask = kubernetesClient.ListKustomizationsWithHttpMessagesAsync();

    await foreach (var (_, kustomization) in responseTask.WatchAsync<V1CustomResourceDefinition, object>(cancellationToken: token))
    {
      string? kustomizationName = kustomization?.Metadata.Name ??
        throw new InvalidOperationException("🚨 Kustomization name is null");
      if (!_kustomizations.Add(kustomizationName))
      {
        if (_successfulKustomizations.Count == _kustomizations.Count)
        {
          var totalTimeElapsed = _stopwatchTotal.Elapsed;
          int minutes = totalTimeElapsed.Minutes;
          int seconds = totalTimeElapsed.Seconds;
          Console.WriteLine($"✔ All kustomizations are ready! ({minutes}m {seconds}s)");
          return 0;
        }
        else if (_successfulKustomizations.Contains(kustomizationName))
        {
          continue;
        }
      }
      var statusConditionStatuses = kustomization?.Status.Conditions.Select(condition => condition.Status) ??
        throw new InvalidOperationException("🚨 Kustomization status is null");
      string? statusConditionType = kustomization?.Status.Conditions.FirstOrDefault()?.Type ??
        throw new InvalidOperationException("🚨 Kustomization status is null");
      switch (statusConditionType)
      {
        case "Failed":
          return HandleFailedStatus(kustomization, kustomizationName);
        case "Ready" when statusConditionStatuses.All(status => status.Equals("True", StringComparison.Ordinal)):
          HandleReadyStatus(kustomizationName);
          break;
        default:
          if (HandleFailedStatusConditions(kustomization, kustomizationName) != 0)
          {
            return 1;
          }
          HandleOtherStatus(kustomizationName, statusConditionType);
          break;
      }
    }
    return 0;
  }

  static int HandleFailedStatusConditions(V1CustomResourceDefinition? kustomization, string kustomizationName)
  {
    bool isFailed = kustomization?.Status.Conditions.Any(condition =>
      condition.Status.Equals("False", StringComparison.Ordinal) &&
      !condition.Reason.Equals("HealthCheckFailed", StringComparison.Ordinal)
    ) ?? false;

    return isFailed ? HandleFailedStatus(kustomization, kustomizationName) : 0;
  }

  void HandleOtherStatus(string kustomizationName, string statusConditionType)
  {
    var timeElapsed = _stopwatch.Elapsed;
    int minutes = timeElapsed.Minutes;
    int seconds = timeElapsed.Seconds;
    Console.WriteLine($"◎ Waiting for kustomization '{kustomizationName}' to be ready ({minutes}m {seconds}s)");
    Console.WriteLine($"  Current status: {statusConditionType}");
  }

  void HandleReadyStatus(string kustomizationName)
  {
    Console.WriteLine($"✔ Kustomization '{kustomizationName}' is ready!");
    _ = _successfulKustomizations.Add(kustomizationName);
    _stopwatch.Restart();
  }

  static int HandleFailedStatus(V1CustomResourceDefinition? kustomization, string kustomizationName)
  {
    string? message = kustomization?.Status.Conditions.FirstOrDefault()?.Message;
    Console.WriteLine($"✕ Kustomization '{kustomizationName}' failed with message: {message}");
    return 1;
  }

  static Kubernetes CreateKubernetesClientFromClusterName(string context)
  {
    var kubeConfig = KubernetesClientConfiguration.LoadKubeConfig();
    var config = KubernetesClientConfiguration.BuildConfigFromConfigObject(kubeConfig, context);
    return new Kubernetes(config);
  }
}
