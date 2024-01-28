using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Lint.Handlers;
using KSail.Options;

namespace KSail.Commands.Lint;

sealed class KSailLintCommand : Command
{
  readonly NameArgument _nameArgument = new();
  readonly ManifestsOption _manifestsOption = new() { IsRequired = true };
  internal KSailLintCommand() : base(
   "lint", "Lint manifest files"
  )
  {
    AddArgument(_nameArgument);
    AddOption(_manifestsOption);
    this.SetHandler(KSailLintCommandHandler.HandleAsync, _nameArgument, _manifestsOption);
  }
}
