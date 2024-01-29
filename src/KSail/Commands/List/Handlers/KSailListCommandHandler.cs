using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.List.Handlers;

sealed class KSailListCommandHandler(IKubernetesDistributionProvisioner kubernetesDistributionProvisioner)
{
  readonly IKubernetesDistributionProvisioner _kubernetesDistributionProvisioner = kubernetesDistributionProvisioner;

  internal Task<string> HandleAsync() => _kubernetesDistributionProvisioner.ListAsync();
}
