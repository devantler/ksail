using KSail.CLIWrappers;
using KSail.Enums;

namespace KSail.Provisioners.KubernetesDistribution;

sealed class K3dProvisioner() : IKubernetesDistributionProvisioner
{
  public Task<KubernetesDistributionType> GetKubernetesDistributionTypeAsync() => Task.FromResult(KubernetesDistributionType.K3d);
  public async Task ProvisionAsync(string clusterName, string configPath)
  {
    Console.WriteLine($"🚀 Provisioning K3d cluster '{clusterName}'...");
    await K3dCLIWrapper.CreateClusterAsync(clusterName, configPath);
    Console.WriteLine();
  }

  public Task DeprovisionAsync(string clusterName)
  {
    Console.WriteLine($"🔥 Destroying K3d cluster '{clusterName}'...");
    return K3dCLIWrapper.DeleteClusterAsync(clusterName);
  }

  public Task<string> ListAsync() => _ = K3dCLIWrapper.ListClustersAsync();

  public Task<bool> ExistsAsync(string clusterName) => K3dCLIWrapper.GetClusterAsync(clusterName);
}
