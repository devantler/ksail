using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.List.Handlers;

sealed class KSailListCommandHandler(IKubernetesDistributionProvisioner kubernetesDistributionProvisioner)
{
  readonly IKubernetesDistributionProvisioner _kubernetesDistributionProvisioner = kubernetesDistributionProvisioner;

  internal Task<(int exitCode, string result)> HandleAsync(CancellationToken token) => _kubernetesDistributionProvisioner.ListAsync(token);
}
