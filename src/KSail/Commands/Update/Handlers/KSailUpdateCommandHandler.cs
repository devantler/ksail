using KSail.Commands.Lint.Handlers;
using KSail.Provisioners.GitOps;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Update.Handlers;

class KSailUpdateCommandHandler(IKubernetesDistributionProvisioner kubernetesDistributionProvisioner, IGitOpsProvisioner gitOpsProvisioner)
{
  readonly IKubernetesDistributionProvisioner _kubernetesDistributionProvisioner = kubernetesDistributionProvisioner;
  readonly IGitOpsProvisioner _gitOpsProvisioner = gitOpsProvisioner;
  internal async Task HandleAsync(string clusterName, string manifestsPath, bool noLint, bool noReconcile)
  {
    if (!noLint)
    {
      await KSailLintCommandHandler.HandleAsync(clusterName, manifestsPath);
    }
    Console.WriteLine($"📥 Pushing manifests to {clusterName}...");
    var kubernetesDistributionType = await _kubernetesDistributionProvisioner.GetKubernetesDistributionTypeAsync();
    await _gitOpsProvisioner.PushManifestsAsync($"{kubernetesDistributionType}-{clusterName}", $"oci://localhost:5050/{clusterName}", manifestsPath);
    if (!noReconcile)
    {
      Console.WriteLine();
      Console.WriteLine($"📥 Reconciling manifests on {clusterName}...");
      await _gitOpsProvisioner.ReconcileAsync($"{kubernetesDistributionType}-{clusterName}");
    }
    Console.WriteLine("");
  }
}
