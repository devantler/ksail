using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Init.Handlers;
using KSail.Options;

namespace KSail.Commands.Init;

sealed class KSailInitCommand : Command
{
  readonly ClusterNameArgument _clusterNameArgument = new();
  readonly ManifestsOption _manifestsOption = new() { IsRequired = true };
  public KSailInitCommand() : base("init", "Initialize a cluster")
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
        var handler = new KSailInitCommandHandler(clusterName, manifests);
        context.ExitCode = await handler.HandleAsync(token);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
