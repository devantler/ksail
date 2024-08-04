using KSail.Enums;

namespace KSail.Provisioners.KubernetesDistribution;

interface IKubernetesDistributionProvisioner
{
  Task<KubernetesDistributionType> GetKubernetesDistributionTypeAsync();
  Task<int> ProvisionAsync(string clusterName, string configPath, CancellationToken token);
  Task<int> DeprovisionAsync(string clusterName, CancellationToken token);
  Task<(int exitCode, string result)> ListAsync(CancellationToken token);
  Task<(int exitCode, bool result)> ExistsAsync(string clusterName, CancellationToken token);
}
