using System.CommandLine;
using KSail.Commands.Lint.Handlers;
using KSail.Options;

namespace KSail.Commands.Lint;

sealed class KSailLintCommand : Command
{
  readonly NameOption _nameOption = new("name of the cluster to lint") { IsRequired = true };
  readonly ManifestsPathOption _manifestsPathOption = new() { IsRequired = true };
  internal KSailLintCommand() : base(
   "lint", "lint manifest files"
  )
  {
    AddOption(_manifestsPathOption);
    AddOption(_nameOption);
    this.SetHandler(KSailLintCommandHandler.HandleAsync, _nameOption, _manifestsPathOption);
  }
}
