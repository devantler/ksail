using System.CommandLine;
using KSail.Commands.Lint.Handlers;
using KSail.Options;

namespace KSail.Commands.Lint;

sealed class KSailLintCommand : Command
{
  readonly NameOption nameOption = new("Name of the cluster to lint") { IsRequired = true };
  readonly ManifestsOption manifestsOption = new() { IsRequired = true };
  internal KSailLintCommand(IConsole console) : base(
   "lint", "Lint manifest files"
  )
  {
    AddOption(manifestsOption);
    AddOption(nameOption);
    this.SetHandler(new KSailLintCommandHandler(console).HandleAsync, nameOption, manifestsOption);
  }
}
