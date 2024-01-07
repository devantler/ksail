using System.CommandLine;
using KSail.Commands.Lint.Handlers;
using KSail.Options;

namespace KSail.Commands.Lint;

sealed class KSailLintCommand : Command
{
  readonly ManifestsPathOption _manifestsPathOption = new();
  internal KSailLintCommand() : base(
   "lint", "lint manifest files"
  )
  {
    AddOption(_manifestsPathOption);
    this.SetHandler(KSailLintCommandHandler.Handle, _manifestsPathOption);
  }
}
