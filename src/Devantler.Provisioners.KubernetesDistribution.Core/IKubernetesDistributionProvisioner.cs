using KSail.Enums;

namespace KSail.Provisioners.KubernetesDistribution;

interface IKubernetesDistributionProvisioner
{
  Task<KubernetesDistributionType> GetKubernetesDistributionTypeAsync();
  Task<int> ProvisionAsync(string clusterName, string configPath, CancellationToken token);
  Task<int> DeprovisionAsync(string clusterName, CancellationToken token);
  Task<(int ExitCode, string Result)> ListAsync(CancellationToken token);
  Task<(int ExitCode, bool Result)> ExistsAsync(string clusterName, CancellationToken token);
}
