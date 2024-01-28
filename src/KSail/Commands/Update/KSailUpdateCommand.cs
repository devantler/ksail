using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Update.Handlers;
using KSail.Commands.Update.Options;
using KSail.Enums;
using KSail.Options;
using KSail.Services.Provisioners.GitOps;
using KSail.Services.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Update;

sealed class KSailUpdateCommand : Command
{
  readonly KubernetesDistributionProvisionerBinder _kubernetesDistributionProvisionerBinder = new(KubernetesDistributionType.K3d);
  readonly GitOpsProvisionerBinder _gitOpsProvisionerBinder = new(GitOpsType.Flux);
  readonly ClusterNameArgument _clusterNameArgument = new() { Arity = ArgumentArity.ExactlyOne };
  readonly ManifestsOption _manifestsOption = new() { IsRequired = true };
  readonly NoLintOption _noLintOption = new();
  readonly NoReconcileOption _noReconcileOption = new();
  internal KSailUpdateCommand() : base(
    "update",
    "Update manifests in an OCI registry"
  )
  {
    AddArgument(_clusterNameArgument);
    AddOption(_manifestsOption);
    AddOption(_noLintOption);
    AddOption(_noReconcileOption);
    this.SetHandler(async (kubernetesDistributionProvisioner, gitOpsProvisioner, clusterName, manifests, noLint, noReconcile) =>
    {
      var handler = new KSailUpdateCommandHandler(kubernetesDistributionProvisioner, gitOpsProvisioner);
      await handler.HandleAsync(clusterName, manifests, noLint, noReconcile);
    }, _kubernetesDistributionProvisionerBinder, _gitOpsProvisionerBinder, _clusterNameArgument, _manifestsOption, _noLintOption, _noReconcileOption);
  }
}
