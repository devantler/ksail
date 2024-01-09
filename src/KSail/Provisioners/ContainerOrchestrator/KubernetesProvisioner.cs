using System.Text;
using k8s;
using k8s.Models;

namespace KSail.Provisioners.ContainerOrchestrator;

sealed class KubernetesProvisioner : IContainerOrchestratorProvisioner, IDisposable
{
  readonly Kubernetes kubernetesClient = new(KubernetesClientConfiguration.BuildDefaultConfig());

  /// <inheritdoc/>
  internal async Task CreateNamespaceAsync(string name)
  {
    Console.WriteLine($"🌐 Creating '{name}' namespace...");
    var fluxSystemNamespace = new V1Namespace
    {
      ApiVersion = "v1",
      Kind = "Namespace",
      Metadata = new V1ObjectMeta
      {
        Name = name
      }
    };
    _ = await kubernetesClient.CreateNamespaceAsync(fluxSystemNamespace);
    Console.WriteLine("✔ Namespace created...");
    Console.WriteLine();
  }

  internal async Task CreateSecretAsync(string name, Dictionary<string, string> data, string @namespace = "default")
  {
    Console.WriteLine($"► Deploying '{name}' secret to '{@namespace}' namespace...");
    var sopsGpgSecret = new V1Secret
    {
      ApiVersion = "v1",
      Kind = "Secret",
      Metadata = new V1ObjectMeta
      {
        Name = "sops-gpg",
        NamespaceProperty = "flux-system"
      },
      Type = "Opaque",
      Data = data.ToDictionary(
        pair => pair.Key,
        pair => Encoding.UTF8.GetBytes(pair.Value)
      )
    };
    _ = await kubernetesClient.CreateNamespacedSecretAsync(sopsGpgSecret, "flux-system");
  }

  public void Dispose()
  {
    kubernetesClient.Dispose();
    GC.SuppressFinalize(this);
  }
}
