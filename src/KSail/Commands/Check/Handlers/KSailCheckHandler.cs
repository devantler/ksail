using System.Globalization;
using k8s;
using k8s.Models;

namespace KSail.Commands.Check.Handlers;

static class KSailCheckHandler
{
  internal static async Task HandleAsync(string name, CancellationToken cancellationToken)
  {
    // Load the kubeconfig file
    var kubeConfig = KubernetesClientConfiguration.LoadKubeConfig();

    // Find the context by name
    var context = kubeConfig.Contexts.FirstOrDefault(c => c.Name == name);

    if (context == null)
    {
      Console.WriteLine($"❌ Could not find a context with the name '{name}' in the kubeconfig file.");
      Environment.Exit(1);
    }

    // Create a KubernetesClientConfiguration object from the context
    var config = KubernetesClientConfiguration.BuildConfigFromConfigObject(kubeConfig, name);

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

    //Check that all kustomizations have been created and return success if so.
    var kustomizations = new List<string>();
    var successFullKustomizations = new List<string>();
    await foreach (var (type, kustomization) in listResponse.WatchAsync<V1CustomResourceDefinition, object>(cancellationToken: cancellationToken))
    {
      // Assuming the status of the Kustomization is stored in the 'status' field
      string? kustomizationName = kustomization?.Metadata.Name;
      string? statusName = kustomization?.Status.Conditions.FirstOrDefault()?.Type;
      if (string.IsNullOrEmpty(kustomizationName))
      {
        Console.WriteLine("❌ The kustomization name is null or empty.");
        Environment.Exit(1);
      }
      if (string.IsNullOrEmpty(statusName))
      {
        Console.WriteLine("❌ The status name is null or empty.");
        Environment.Exit(1);
      }
      if (statusName == "Failed")
      {
        Console.WriteLine($"❌ Kustomization '{kustomizationName}' failed!");
        string? message = kustomization?.Status.Conditions.FirstOrDefault()?.Message;
        Console.WriteLine($"❌ {message}");
        Environment.Exit(1);
      }
      if (successFullKustomizations.Contains(kustomizationName))
      {
        continue;
      }
      if (!kustomizations.Contains(kustomizationName))
      {
        kustomizations.Add(kustomizationName);
      }
      if (statusName == "Ready")
      {
        Console.WriteLine($"✅ Kustomization '{kustomizationName}' is ready!");
        successFullKustomizations.Add(kustomizationName);
        continue;
      }
      if (successFullKustomizations.Count == kustomizations.Count)
      {
        Console.WriteLine($"✅ All kustomizations are ready!");
        Environment.Exit(0);
      }
      Console.WriteLine($"🔁 Waiting for kustomization '{kustomizationName}' to be ready. It is currently {statusName.ToLower(CultureInfo.InvariantCulture)}...");
    }
  }
}
