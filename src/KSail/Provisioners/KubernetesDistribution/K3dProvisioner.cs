using KSail.CLIWrappers;
using KSail.Enums;

namespace KSail.Provisioners.KubernetesDistribution;

sealed class K3dProvisioner() : IKubernetesDistributionProvisioner
{
  public Task<KubernetesDistributionType> GetKubernetesDistributionTypeAsync() => Task.FromResult(KubernetesDistributionType.K3d);

  public async Task<int> ProvisionAsync(string clusterName, string configPath, CancellationToken token) =>
    await K3dCLIWrapper.CreateClusterAsync(clusterName, configPath, token) != 0 ? 1 : 0;

  public Task<int> DeprovisionAsync(string clusterName, CancellationToken token) =>
    K3dCLIWrapper.DeleteClusterAsync(clusterName, token);

  public Task<(int ExitCode, string Result)> ListAsync(CancellationToken token) => _ = K3dCLIWrapper.ListClustersAsync(token);

  public Task<(int ExitCode, bool Result)> ExistsAsync(string clusterName, CancellationToken token) => K3dCLIWrapper.GetClusterAsync(clusterName, token);
}
