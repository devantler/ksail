using System.CommandLine;
using KSail.Commands.Lint.Handlers;
using KSail.Options;

namespace KSail.Commands.Lint;

sealed class KSailLintCommand : Command
{
  readonly NameOption nameOption = new("Name of the cluster to lint") { IsRequired = true };
  readonly ManifestsOption manifestsOption = new() { IsRequired = true };
  internal KSailLintCommand() : base(
   "lint", "Lint manifest files"
  )
  {
    AddOption(manifestsOption);
    AddOption(nameOption);
    this.SetHandler(KSailLintCommandHandler.HandleAsync, nameOption, manifestsOption);
  }
}
