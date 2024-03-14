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

  internal async Task<int> HandleAsync(string context, int timeout, CancellationToken token, string? kubeconfig = null)
  {
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
          return HandleSuccesfulKustomizations();
        }
        else if (_successfulKustomizations.Contains(kustomizationName))
        {
          continue;
        }
        else if (_stopwatch.Elapsed.TotalSeconds >= timeout)
        {
          Console.WriteLine($"✕ Kustomization '{kustomizationName}' did not become ready within the specified time limit of {timeout} seconds.");
          return 1;
        }
      }
      var statusConditions = kustomization?.Status.Conditions ??
        throw new InvalidOperationException("🚨 Kustomization status conditions are null");
      if (HasDependencies(statusConditions))
      {
        continue;
      }
      bool isReady = true;
      foreach (var statusCondition in statusConditions)
      {
        switch (statusCondition.Type)
        {
          case "Stalled":
          case "Failed" when IsCritical(statusCondition):
            return HandleFailedStatus(statusCondition, kustomizationName);
          case "Ready":
          case "Healthy":
            break;
          default:
            isReady = false;
            break;
        }
      }
      if (isReady)
      {
        HandleReadyStatus(kustomizationName);
      }
      else
      {
        HandleOtherStatus(kustomizationName);
      }
    }
    return 0;
  }

  static bool IsCritical(V1CustomResourceDefinitionCondition statusCondition) =>
    statusCondition.Status.Equals("False", StringComparison.Ordinal) &&
    !statusCondition.Reason.Equals("HealthCheckFailed", StringComparison.Ordinal);

  static bool HasDependencies(IList<V1CustomResourceDefinitionCondition> statusConditions) =>
    statusConditions.FirstOrDefault()?.Reason.Equals("DependencyNotReady", StringComparison.Ordinal) ?? false;

  int HandleSuccesfulKustomizations()
  {
    var totalTimeElapsed = _stopwatchTotal.Elapsed;
    int minutes = totalTimeElapsed.Minutes;
    int seconds = totalTimeElapsed.Seconds;
    Console.WriteLine($"✔ All kustomizations are ready! ({minutes}m {seconds}s)");
    return 0;
  }

  void HandleOtherStatus(string kustomizationName)
  {
    var timeElapsed = _stopwatch.Elapsed;
    int minutes = timeElapsed.Minutes;
    int seconds = timeElapsed.Seconds;
    Console.WriteLine($"◎ Waiting for kustomization '{kustomizationName}' to become ready ({minutes}m {seconds}s)");
  }

  void HandleReadyStatus(string kustomizationName)
  {
    Console.WriteLine($"✔ Kustomization '{kustomizationName}' is ready!");
    _ = _successfulKustomizations.Add(kustomizationName);
    _stopwatch.Restart();
  }

  static int HandleFailedStatus(V1CustomResourceDefinitionCondition statusCondition, string kustomizationName)
  {
    string? message = statusCondition.Message;
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
