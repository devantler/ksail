using System.Diagnostics;
using k8s;
using k8s.Models;
using KSail.Exceptions;
using KSail.Extensions;

namespace KSail.Commands.Check.Handlers;

class KSailCheckCommandHandler()
{
  readonly HashSet<string> _kustomizations = [];
  readonly HashSet<string> _successFullKustomizations = [];
  readonly Stopwatch _stopwatch = Stopwatch.StartNew();

  internal async Task HandleAsync(string context, int timeout, CancellationToken cancellationToken)
  {
    Console.WriteLine("👀 Checking the status of the cluster...");
    var kubernetesClient = CreateKubernetesClientFromClusterName(context);
    var responseTask = kubernetesClient.ListKustomizationsWithHttpMessagesAsync(cancellationToken);

    await foreach (var (type, kustomization) in responseTask.WatchAsync<V1CustomResourceDefinition, object>(cancellationToken: cancellationToken))
    {
      string? kustomizationName = kustomization?.Metadata.Name ??
        throw new InvalidOperationException("🚨 Kustomization name is null");
      string? statusConditionStatus = kustomization?.Status.Conditions.FirstOrDefault()?.Status ??
        throw new InvalidOperationException("🚨 Kustomization status is null");
      string? statusConditionType = kustomization?.Status.Conditions.FirstOrDefault()?.Type ??
        throw new InvalidOperationException("🚨 Kustomization status is null");

      if (!_kustomizations.Add(kustomizationName))
      {
        if (_successFullKustomizations.Count == _kustomizations.Count)
        {
          Console.WriteLine("✔ All kustomizations are ready!");
          return;
        }
        else if (_stopwatch.Elapsed.TotalSeconds >= timeout)
        {
          throw new TimeoutException($"🚨 Kustomization '{kustomizationName}' did not become ready within the specified time limit of {timeout} seconds.");
        }
        else if (_successFullKustomizations.Contains(kustomizationName))
        {
          continue;
        }
      }
      if (statusConditionStatus.Equals("false", StringComparison.OrdinalIgnoreCase))
      {
        continue;
      }
      switch (statusConditionType)
      {
        //TODO: Implement check command with condition[1].type == healthy. This should work for all kustomizations.
        case "Failed":
          HandleFailedStatus(kustomization, kustomizationName);
          break;
        case "Ready":
          HandleReadyStatus(kustomizationName);
          break;
        default:
          Console.WriteLine($"◎ Waiting for kustomization '{kustomizationName}' to be ready...");
          Console.WriteLine($"  Current status: {statusConditionType}");
          foreach (var condition in kustomization?.Status.Conditions ?? Enumerable.Empty<V1CustomResourceDefinitionCondition>())
          {
            Console.WriteLine($"  {condition.Message}");
          }
          Console.WriteLine($"  Elapsed time: {_stopwatch.Elapsed.TotalSeconds:0}s out of {timeout}s");
          break;
      }
    }
  }

  void HandleReadyStatus(string kustomizationName)
  {
    Console.WriteLine($"✔ Kustomization '{kustomizationName}' is ready! Resetting timer...");
    _ = _successFullKustomizations.Add(kustomizationName);
    _stopwatch.Restart();
  }

  static void HandleFailedStatus(V1CustomResourceDefinition? kustomization, string kustomizationName)
  {
    string? message = kustomization?.Status.Conditions.FirstOrDefault()?.Message;
    throw new KSailException($"🚨 Kustomization '{kustomizationName}' failed with message: {message}");
  }

  static Kubernetes CreateKubernetesClientFromClusterName(string context)
  {
    var kubeConfig = KubernetesClientConfiguration.LoadKubeConfig();
    var config = KubernetesClientConfiguration.BuildConfigFromConfigObject(kubeConfig, context);
    return new Kubernetes(config);
  }
}
