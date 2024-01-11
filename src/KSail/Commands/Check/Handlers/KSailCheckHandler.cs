using System.Globalization;
using k8s;
using k8s.Models;

namespace KSail.Commands.Check.Handlers;

static class KSailCheckHandler
{
  static readonly Kubernetes kubernetesClient = new(KubernetesClientConfiguration.BuildDefaultConfig());
  internal static async Task HandleAsync(string name, CancellationToken cancellationToken)
  {
    // Wait for the cluster to be ready

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
        var message = kustomization?.Status.Conditions.FirstOrDefault()?.Message;
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
