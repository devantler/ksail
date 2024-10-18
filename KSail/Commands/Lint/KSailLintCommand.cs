using System.CommandLine;
using KSail.Commands.Lint.Handlers;
using KSail.Options;

namespace KSail.Commands.Lint;

sealed class KSailLintCommand : Command
{
  readonly NameOption _clusterNameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ManifestsOption _manifestsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  internal KSailLintCommand() : base(
   "lint", "Lint manifests for a cluster"
  )
  {
    AddOption(_clusterNameOption);
    AddOption(_manifestsOption);
    this.SetHandler(async (context) =>
    {
      string clusterName = context.ParseResult.GetValueForOption(_clusterNameOption)!;
      string manifests = context.ParseResult.GetValueForOption(_manifestsOption)!;
      var cancellationToken = context.GetCancellationToken();
      try
      {
        context.ExitCode = await KSailLintCommandHandler.HandleAsync(clusterName, manifests, cancellationToken).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
