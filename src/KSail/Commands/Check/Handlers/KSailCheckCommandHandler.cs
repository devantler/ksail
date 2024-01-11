using System.Data;
using System.Diagnostics;
using System.Globalization;
using k8s;
using k8s.Models;
using KSail.Extensions;

namespace KSail.Commands.Check.Handlers;

static class KSailCheckCommandHandler
{
  static readonly List<string> kustomizations = [];
  static readonly List<string> successFullKustomizations = [];
  internal static async Task HandleAsync(string name, int timeout, CancellationToken cancellationToken)
  {
    Console.WriteLine("👀 Checking the status the cluster...");
    var kubernetesClient = CreateKubernetesClientFromClusterName(name);
    var responseTask = kubernetesClient.ListKustomizationsWithHttpMessagesAsync(cancellationToken);
    var stopwatch = Stopwatch.StartNew();

    await foreach (var (type, kustomization) in responseTask.WatchAsync<V1CustomResourceDefinition, object>(cancellationToken: cancellationToken))
    {
      string? kustomizationName = kustomization?.Metadata.Name ??
        throw new NoNullAllowedException("Kustomization name is null");
      string? statusName = kustomization?.Status.Conditions.FirstOrDefault()?.Type ??
        throw new NoNullAllowedException("Kustomization status is null");
      if (statusName == "Failed")
      {
        Console.WriteLine($"✕ Kustomization '{kustomizationName}' failed!");
        string? message = kustomization?.Status.Conditions.FirstOrDefault()?.Message;
        Console.WriteLine($"✕ {message}");
        Environment.Exit(1);
      }
      if (!kustomizations.Contains(kustomizationName))
        kustomizations.Add(kustomizationName);
      if (successFullKustomizations.Count == kustomizations.Count)
      {
        Console.WriteLine("✔ All kustomizations are ready!");
        Environment.Exit(0);
      }
      if (successFullKustomizations.Contains(kustomizationName))
        continue;

      if (statusName == "Ready")
      {
        Console.WriteLine($"✔ Kustomization '{kustomizationName}' is ready!");
        successFullKustomizations.Add(kustomizationName);
        stopwatch.Restart(); // Reset the timeout
        continue;
      }
      Console.WriteLine($"► Waiting for kustomization '{kustomizationName}' to be ready. It is currently {statusName?.ToLower(CultureInfo.InvariantCulture)}...");

      if (stopwatch.Elapsed.TotalSeconds >= timeout)
      {
        Console.WriteLine($"✕ Timeout reached. Kustomization '{kustomizationName}' did not become ready within the specified timeout of {timeout} seconds.");
        Environment.Exit(1);
      }
    }
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
