using System.CommandLine;
using Devantler.KubernetesProvisioner.Cluster.Core;
using Devantler.KubernetesProvisioner.Cluster.K3d;
using Devantler.KubernetesProvisioner.GitOps.Flux;
using KSail.Commands.Update.Handlers;
using KSail.Commands.Update.Options;
using KSail.Options;

namespace KSail.Commands.Update;

sealed class KSailUpdateCommand : Command
{
  readonly NameOption _clusterNameOption = new() { Arity = ArgumentArity.ExactlyOne };
  readonly ManifestsOption _manifestsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly NoLintOption _noLintOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly NoReconcileOption _noReconcileOption = new() { Arity = ArgumentArity.ZeroOrOne };
  internal KSailUpdateCommand() : base(
    "update",
    "Update a cluster"
  )
  {
    AddOption(_clusterNameOption);
    AddOption(_manifestsOption);
    AddOption(_noLintOption);
    AddOption(_noReconcileOption);
    this.SetHandler(async (context) =>
    {
      IKubernetesClusterProvisioner clusterProvisioner = new K3dProvisioner();
      var gitOpsProvisioner = new FluxProvisioner();

      string clusterName = context.ParseResult.GetValueForOption(_clusterNameOption);
      string manifests = context.ParseResult.GetValueForOption(_manifestsOption);
      bool noLint = context.ParseResult.GetValueForOption(_noLintOption);
      bool noReconcile = context.ParseResult.GetValueForOption(_noReconcileOption);

      var cancellationToken = context.GetCancellationToken();
      var handler = new KSailUpdateCommandHandler(clusterProvisioner, gitOpsProvisioner);
      try
      {
        context.ExitCode = await handler.HandleAsync(clusterName, manifests, noLint, noReconcile, cancellationToken).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
