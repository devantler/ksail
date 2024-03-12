using System.Diagnostics;
using System.Globalization;
using k8s;
using k8s.Models;
using KSail.Extensions;

namespace KSail.Commands.Check.Handlers;

class KSailCheckCommandHandler()
{
  readonly HashSet<string> _kustomizations = [];
  readonly HashSet<string> _successFullKustomizations = [];
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
      string? statusConditionType = kustomization?.Status.Conditions.FirstOrDefault()?.Type ??
        throw new InvalidOperationException("🚨 Kustomization status is null");

      if (!_kustomizations.Add(kustomizationName))
      {
        if (_successFullKustomizations.Count == _kustomizations.Count)
        {
          var totalTimeElapsed = _stopwatchTotal.Elapsed;
          Console.WriteLine($"✔ All kustomizations are ready! Elapsed time: {totalTimeElapsed.ToString(@"m\:ss", CultureInfo.InvariantCulture)}");
          return 0;
        }
        else if (_successFullKustomizations.Contains(kustomizationName))
        {
          continue;
        }
      }
      switch (statusConditionType)
      {
        case "Failed":
          return HandleFailedStatus(kustomization, kustomizationName);
        case "Ready":
          HandleReadyStatus(kustomizationName);
          break;
        default:
          Console.WriteLine($"◎ Waiting for kustomization '{kustomizationName}' to be ready");
          Console.WriteLine($"  Current status: {statusConditionType}");
          var timeElapsed = _stopwatch.Elapsed;
          Console.WriteLine($"  Elapsed time: {timeElapsed.ToString(@"m\:ss", CultureInfo.InvariantCulture)}");

          bool isFailed = kustomization?.Status.Conditions.Any(condition =>
            condition.Status.Equals("False", StringComparison.Ordinal) &&
            !condition.Reason.Equals("HealthCheckFailed", StringComparison.Ordinal)
          ) ?? false;

          if (isFailed)
          {
            return HandleFailedStatus(kustomization, kustomizationName);
          }
          break;
      }
    }
    return 0;
  }

  void HandleReadyStatus(string kustomizationName)
  {
    Console.WriteLine($"✔ Kustomization '{kustomizationName}' is ready!");
    _ = _successFullKustomizations.Add(kustomizationName);
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
