using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.List.Handlers;

sealed class KSailListCommandHandler(IKubernetesDistributionProvisioner kubernetesDistributionProvisioner)
{
  readonly IKubernetesDistributionProvisioner _kubernetesDistributionProvisioner = kubernetesDistributionProvisioner;

  internal Task<(int ExitCode, string Result)> HandleAsync(CancellationToken token) => _kubernetesDistributionProvisioner.ListAsync(token);
}
