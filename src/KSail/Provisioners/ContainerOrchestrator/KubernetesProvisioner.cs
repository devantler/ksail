using System.Text;
using k8s;
using k8s.Models;
using KSail.Enums;

namespace KSail.Provisioners.ContainerOrchestrator;

sealed class KubernetesProvisioner : IContainerOrchestratorProvisioner, IDisposable
{
  Kubernetes? _kubernetesClient;

  /// <inheritdoc/>
  public async Task CreateNamespaceAsync(string context, string name)
  {
    var kubeConfig = KubernetesClientConfiguration.LoadKubeConfig();
    var config = KubernetesClientConfiguration.BuildConfigFromConfigObject(kubeConfig, context);
    _kubernetesClient = new Kubernetes(config);
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
    _ = await _kubernetesClient.CreateNamespaceAsync(fluxSystemNamespace);
    Console.WriteLine("✔ Namespace created...");
    Console.WriteLine();
  }

  public async Task CreateSecretAsync(string context, string name, Dictionary<string, string> data, string @namespace = "default")
  {
    var kubeConfig = KubernetesClientConfiguration.LoadKubeConfig();
    var config = KubernetesClientConfiguration.BuildConfigFromConfigObject(kubeConfig, context);
    _kubernetesClient = new Kubernetes(config);
    Console.WriteLine($"► Deploying '{name}' secret to '{@namespace}' namespace");
    var sopsGpgSecret = new V1Secret
    {
      ApiVersion = "v1",
      Kind = "Secret",
      Metadata = new V1ObjectMeta
      {
        Name = "sops-age",
        NamespaceProperty = "flux-system"
      },
      Type = "Opaque",
      Data = data.ToDictionary(
        pair => pair.Key,
        pair => Encoding.UTF8.GetBytes(pair.Value)
      )
    };
    _ = await _kubernetesClient.CreateNamespacedSecretAsync(sopsGpgSecret, "flux-system");
  }

  public Task<ContainerOrchestratorType> GetContainerOrchestratorTypeAsync() => Task.FromResult(ContainerOrchestratorType.Kubernetes);

  public void Dispose()
  {
    _kubernetesClient?.Dispose();
    GC.SuppressFinalize(this);
  }
}
