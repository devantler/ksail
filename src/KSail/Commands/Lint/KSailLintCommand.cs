using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Lint.Handlers;
using KSail.Options;

namespace KSail.Commands.Lint;

sealed class KSailLintCommand : Command
{
  readonly ClusterNameArgument _clusterNameArgument = new();
  readonly ManifestsOption _manifestsOption = new() { IsRequired = true };
  internal KSailLintCommand() : base(
   "lint", "Lint manifest files"
  )
  {
    AddArgument(_clusterNameArgument);
    AddOption(_manifestsOption);
    this.SetHandler(KSailLintCommandHandler.HandleAsync, _clusterNameArgument, _manifestsOption);
  }
}
