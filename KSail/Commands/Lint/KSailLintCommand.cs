using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Lint.Handlers;
using KSail.Options;

namespace KSail.Commands.Lint;

sealed class KSailLintCommand : Command
{
  readonly ClusterNameArgument _clusterNameArgument = new() { Arity = ArgumentArity.ExactlyOne };
  readonly ManifestsOption _manifestsOption = new() { IsRequired = true };
  internal KSailLintCommand() : base(
   "lint", "Lint manifests for a cluster"
  )
  {
    AddArgument(_clusterNameArgument);
    AddOption(_manifestsOption);
    this.SetHandler(async (context) =>
    {
      string clusterName = context.ParseResult.GetValueForArgument(_clusterNameArgument);
      string manifests = context.ParseResult.GetValueForOption(_manifestsOption) ??
        throw new InvalidOperationException("🚨 Manifests path is 'null'");
      var token = context.GetCancellationToken();
      try
      {
        context.ExitCode = await KSailLintCommandHandler.HandleAsync(clusterName, manifests, token).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
