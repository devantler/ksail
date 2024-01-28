using System.Data;
using System.Diagnostics;
using k8s;
using k8s.Models;
using KSail.Extensions;

namespace KSail.Commands.Check.Handlers;

class KSailCheckCommandHandler()
{
  static readonly HashSet<string> kustomizations = [];
  static readonly HashSet<string> successFullKustomizations = [];
  static readonly Stopwatch stopwatch = Stopwatch.StartNew();

  internal static async Task HandleAsync(string name, int timeout, CancellationToken cancellationToken)
  {
    Console.WriteLine("👀 Checking the status of the cluster...");
    var kubernetesClient = CreateKubernetesClientFromClusterName(name);
    var responseTask = kubernetesClient.ListKustomizationsWithHttpMessagesAsync(cancellationToken);

    await foreach (var (type, kustomization) in responseTask.WatchAsync<V1CustomResourceDefinition, object>(cancellationToken: cancellationToken))
    {
      string? kustomizationName = kustomization?.Metadata.Name ??
        throw new InvalidOperationException("Kustomization name is null");
      string? statusConditionStatus = kustomization?.Status.Conditions.FirstOrDefault()?.Status ??
        throw new InvalidOperationException("Kustomization status is null");
      string? statusConditionType = kustomization?.Status.Conditions.FirstOrDefault()?.Type ??
        throw new InvalidOperationException("Kustomization status is null");

      if (!kustomizations.Add(kustomizationName))
      {
        if (successFullKustomizations.Count == kustomizations.Count)
        {
          Console.WriteLine("✔ All kustomizations are ready!");
          return;
        }
        else if (stopwatch.Elapsed.TotalSeconds >= timeout)
        {
          Console.WriteLine($"✕ Timeout reached. Kustomization '{kustomizationName}' did not become ready within the specified time limit of {timeout} seconds.");
          Environment.Exit(1);
        }
        else if (successFullKustomizations.Contains(kustomizationName))
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
          Console.WriteLine($"  Elapsed time: {stopwatch.Elapsed.TotalSeconds:0}s out of {timeout}s");
          break;
      }
    }
  }

  static void HandleReadyStatus(string kustomizationName)
  {
    Console.WriteLine($"✔ Kustomization '{kustomizationName}' is ready! Resetting timer...");
    _ = successFullKustomizations.Add(kustomizationName);
    stopwatch.Restart();
  }

  static void HandleFailedStatus(V1CustomResourceDefinition? kustomization, string kustomizationName)
  {
    Console.WriteLine($"✕ Kustomization '{kustomizationName}' failed!");
    string? message = kustomization?.Status.Conditions.FirstOrDefault()?.Message;
    Console.WriteLine($"✕ {message}");
    Environment.Exit(1);
  }

  static Kubernetes CreateKubernetesClientFromClusterName(string name)
  {
    var kubeConfig = KubernetesClientConfiguration.LoadKubeConfig();
    var context = kubeConfig.Contexts.FirstOrDefault(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

    if (context == null)
    {
      Console.WriteLine($"✕ Could not find a context matching the cluster name '{name}' in the kubeconfig file.");
      Console.WriteLine($"  Available contexts are: {string.Join(", ", kubeConfig.Contexts.Select(c => c.Name))}");
      Environment.Exit(1);
    }
    var config = KubernetesClientConfiguration.BuildConfigFromConfigObject(kubeConfig, context.Name);
    return new Kubernetes(config);
  }
}
