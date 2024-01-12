using System.Data;
using System.Diagnostics;
using System.Globalization;
using k8s;
using k8s.Models;
using KSail.Extensions;

namespace KSail.Commands.Check.Handlers;

static class KSailCheckCommandHandler
{
  static readonly HashSet<string> kustomizations = [];
  static readonly HashSet<string> successFullKustomizations = [];
  static readonly Stopwatch stopwatch = Stopwatch.StartNew();

  internal static async Task HandleAsync(string name, int timeout, CancellationToken cancellationToken)
  {
    Console.WriteLine("👀 Checking the status the cluster...");
    var kubernetesClient = CreateKubernetesClientFromClusterName(name);
    var responseTask = kubernetesClient.ListKustomizationsWithHttpMessagesAsync(cancellationToken);

    await foreach (var (type, kustomization) in responseTask.WatchAsync<V1CustomResourceDefinition, object>(cancellationToken: cancellationToken))
    {
      string? kustomizationName = kustomization?.Metadata.Name ??
        throw new NoNullAllowedException("Kustomization name is null");
      string? statusName = kustomization?.Status.Conditions.FirstOrDefault()?.Type ??
        throw new NoNullAllowedException("Kustomization status is null");

      if (successFullKustomizations.Contains(kustomizationName))
        continue;

      if (!kustomizations.Add(kustomizationName))
      {
        if (successFullKustomizations.Count == kustomizations.Count)
        {
          Console.WriteLine("✔ All kustomizations are ready!");
          Environment.Exit(0);
        }
        else if (stopwatch.Elapsed.TotalSeconds >= timeout)
        {
          Console.WriteLine($"✕ Timeout reached. Kustomization '{kustomizationName}' did not become ready within the specified timeout of {timeout} seconds.");
          Environment.Exit(1);
        }
      }

      switch (statusName)
      {
        case "Failed":
          HandleFailedStatus(kustomization, kustomizationName);
          break;
        case "Ready":
          HandleReadyStatus(kustomizationName);
          break;
        default:
          Console.WriteLine($"► Waiting for kustomization '{kustomizationName}' to be ready. It is currently {statusName?.ToLower(CultureInfo.InvariantCulture)}...");
          break;
      }
    }
  }

  static void HandleReadyStatus(string kustomizationName)
  {
    Console.WriteLine($"✔ Kustomization '{kustomizationName}' is ready!");
    _ = successFullKustomizations.Add(kustomizationName);
    stopwatch.Restart();
  }

  static void HandleFailedStatus(V1CustomResourceDefinition kustomization, string kustomizationName)
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
      Console.WriteLine($"   Available contexts are: {string.Join(", ", kubeConfig.Contexts.Select(c => c.Name))}");
      Environment.Exit(1);
    }
    var config = KubernetesClientConfiguration.BuildConfigFromConfigObject(kubeConfig, context.Name);
    return new Kubernetes(config);
  }
}
