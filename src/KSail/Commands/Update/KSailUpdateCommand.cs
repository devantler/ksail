using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Update.Handlers;
using KSail.Commands.Update.Options;
using KSail.Options;
using KSail.Provisioners.GitOps;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Update;

sealed class KSailUpdateCommand : Command
{
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
    this.SetHandler(async (context) =>
    {
      var kubernetesDistributionProvisioner = new K3dProvisioner();
      var gitOpsProvisioner = new FluxProvisioner();

      string clusterName = context.ParseResult.GetValueForArgument(_clusterNameArgument);
      string manifests = context.ParseResult.GetValueForOption(_manifestsOption) ??
        throw new InvalidOperationException("🚨 Manifests path is 'null'");
      bool noLint = context.ParseResult.GetValueForOption(_noLintOption);
      bool noReconcile = context.ParseResult.GetValueForOption(_noReconcileOption);

      var token = context.GetCancellationToken();
      var handler = new KSailUpdateCommandHandler(kubernetesDistributionProvisioner, gitOpsProvisioner);
      try
      {
        context.ExitCode = await handler.HandleAsync(clusterName, manifests, noLint, noReconcile, token);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
