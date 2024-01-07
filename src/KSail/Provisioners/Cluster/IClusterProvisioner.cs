using KSail.Enums;

namespace KSail.Provisioners.Cluster;

interface IClusterProvisioner : IProvisioner
{
  static IClusterProvisioner GetProvisioner(K8sDistribution k8sInDockerBackend)
   => k8sInDockerBackend switch
   {
     K8sDistribution.K3d => new K3dProvisioner(),
     _ => throw new NotSupportedException($"The '{k8sInDockerBackend}' k8s-in-docker backend is not supported.")
   };

  Task ProvisionAsync(string name, bool pullThroughRegistries, string? configPath = null);

  Task DeprovisionAsync(string name);
}
