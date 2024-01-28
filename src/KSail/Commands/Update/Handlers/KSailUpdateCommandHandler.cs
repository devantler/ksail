using KSail.Commands.Lint.Handlers;
using KSail.Services.Provisioners.GitOps;
using KSail.Services.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Update.Handlers;

class KSailUpdateCommandHandler(IKubernetesDistributionProvisioner kubernetesDistributionProvisioner, IGitOpsProvisioner gitOpsProvisioner)
{
  readonly IKubernetesDistributionProvisioner _kubernetesDistributionProvisioner = kubernetesDistributionProvisioner;
  readonly IGitOpsProvisioner _gitOpsProvisioner = gitOpsProvisioner;
  internal async Task HandleAsync(string name, string manifestsPath, bool noLint, bool noReconcile)
  {
    if (!noLint)
    {
      await KSailLintCommandHandler.HandleAsync(name, manifestsPath);
    }
    Console.WriteLine($"📥 Pushing manifests to {name}...");
    var kubernetesDistributionType = await _kubernetesDistributionProvisioner.GetKubernetesDistributionTypeAsync();
    await _gitOpsProvisioner.PushManifestsAsync($"{kubernetesDistributionType}-{name}", $"oci://localhost:5050/{name}", manifestsPath);
    if (!noReconcile)
    {
      Console.WriteLine();
      Console.WriteLine($"📥 Reconciling manifests on {name}...");
      await _gitOpsProvisioner.ReconcileAsync($"{kubernetesDistributionType}-{name}");
    }
    Console.WriteLine("");
  }
}
