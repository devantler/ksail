using System.Globalization;
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
    await _gitOpsProvisioner.PushManifestsAsync($"oci://localhost:5050/{clusterName}", manifestsPath);
    if (!noReconcile)
    {
      var kubernetesDistributionType = await _kubernetesDistributionProvisioner.GetKubernetesDistributionTypeAsync();
      string context = $"{kubernetesDistributionType.ToString()?.ToLower(CultureInfo.InvariantCulture)}-{clusterName}";
      await _gitOpsProvisioner.ReconcileAsync(context);
    }
    Console.WriteLine("");
  }
}
