using System.Data;
using System.Globalization;
using k8s;
using k8s.Models;

namespace KSail.Commands.Check.Handlers;

static class KSailCheckHandler
{
  internal static async Task HandleAsync(string name, CancellationToken cancellationToken)
  {
    var kubeConfig = KubernetesClientConfiguration.LoadKubeConfig();
    var context = kubeConfig.Contexts.FirstOrDefault(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

    if (context == null)
    {
      Console.WriteLine($"❌ Could not find a context matching the cluster name '{name}' in the kubeconfig file.");
      Console.WriteLine($"   Available contexts are: {string.Join(", ", kubeConfig.Contexts.Select(c => c.Name))}");
      Environment.Exit(1);
    }

    // Create a KubernetesClientConfiguration object from the context
    var config = KubernetesClientConfiguration.BuildConfigFromConfigObject(kubeConfig, context.Name);

    // Instantiate the Kubernetes client with the config
    var kubernetesClient = new Kubernetes(config);
    var listResponse = kubernetesClient.CustomObjects.ListNamespacedCustomObjectWithHttpMessagesAsync(
      "kustomize.toolkit.fluxcd.io",
      "v1",
      "flux-system",
      "kustomizations",
      watch: true,
      cancellationToken: cancellationToken
    );

    var kustomizations = new List<string>();
    var successFullKustomizations = new List<string>();
    await foreach (var (type, kustomization) in listResponse.WatchAsync<V1CustomResourceDefinition, object>(cancellationToken: cancellationToken))
    {
      string? kustomizationName = kustomization?.Metadata.Name ??
        throw new NoNullAllowedException("Kustomization name is null");
      string? statusName = kustomization?.Status.Conditions.FirstOrDefault()?.Type ??
        throw new NoNullAllowedException("Kustomization status is null");
      if (statusName == "Failed")
      {
        Console.WriteLine($"❌ Kustomization '{kustomizationName}' failed!");
        string? message = kustomization?.Status.Conditions.FirstOrDefault()?.Message;
        Console.WriteLine($"❌ {message}");
        Environment.Exit(1);
      }
      if (successFullKustomizations.Contains(kustomizationName))
        continue;

      if (!kustomizations.Contains(kustomizationName))
        kustomizations.Add(kustomizationName);

      if (statusName == "Ready")
      {
        Console.WriteLine($"✅ Kustomization '{kustomizationName}' is ready!");
        successFullKustomizations.Add(kustomizationName);
        continue;
      }
      if (successFullKustomizations.Count == kustomizations.Count)
      {
        Console.WriteLine("✅ All kustomizations are ready!");
        Environment.Exit(0);
      }
      Console.WriteLine($"🔁 Waiting for kustomization '{kustomizationName}' to be ready. It is currently {statusName?.ToLower(CultureInfo.InvariantCulture)}...");
    }
  }
}
