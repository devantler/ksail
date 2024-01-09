using System.CommandLine;
using KSail.Commands.Lint.Handlers;
using KSail.Options;

namespace KSail.Commands.Lint;

sealed class KSailLintCommand : Command
{
  readonly NameOption nameOption = new("name of the cluster to lint") { IsRequired = true };
  readonly ManifestsPathOption manifestsPathOption = new() { IsRequired = true };
  internal KSailLintCommand() : base(
   "lint", "lint manifest files"
  )
  {
    AddOption(manifestsPathOption);
    AddOption(nameOption);
    this.SetHandler(KSailLintCommandHandler.HandleAsync, nameOption, manifestsPathOption);
  }
}
