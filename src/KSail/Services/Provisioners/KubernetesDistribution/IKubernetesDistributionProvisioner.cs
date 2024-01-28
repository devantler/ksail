using KSail.Enums;

namespace KSail.Services.Provisioners.KubernetesDistribution;

interface IKubernetesDistributionProvisioner
{
  Task<KubernetesDistributionType> GetKubernetesDistributionTypeAsync();
  Task ProvisionAsync(string clusterName, string configPath);
  Task DeprovisionAsync(string clusterName);
  Task<string> ListAsync();
  Task<bool> ExistsAsync(string clusterName);
}
