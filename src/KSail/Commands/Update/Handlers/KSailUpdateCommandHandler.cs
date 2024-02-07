using KSail.Commands.Lint.Handlers;
using KSail.Provisioners.GitOps;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Update.Handlers;

class KSailUpdateCommandHandler(IKubernetesDistributionProvisioner kubernetesDistributionProvisioner, IGitOpsProvisioner gitOpsProvisioner)
{
  readonly IKubernetesDistributionProvisioner _kubernetesDistributionProvisioner = kubernetesDistributionProvisioner;
  readonly IGitOpsProvisioner _gitOpsProvisioner = gitOpsProvisioner;
  internal async Task<int> HandleAsync(string clusterName, string manifestsPath, bool noLint, bool noReconcile, CancellationToken token)
  {
    if (!noLint && await KSailLintCommandHandler.HandleAsync(clusterName, manifestsPath, token) != 0)
    {
      return 1;
    }
    if (await _gitOpsProvisioner.PushManifestsAsync($"oci://localhost:5050/{clusterName}", manifestsPath, token) != 0)
    {
      return 1;
    }
    if (!noReconcile)
    {
      var kubernetesDistributionType = await _kubernetesDistributionProvisioner.GetKubernetesDistributionTypeAsync();
      string context = $"{kubernetesDistributionType.ToString()?.ToLowerInvariant()}-{clusterName}";
      if (await _gitOpsProvisioner.ReconcileAsync(context, token) != 0)
      {
        return 1;
      }
    }
    Console.WriteLine("");
    return 0;
  }
}
