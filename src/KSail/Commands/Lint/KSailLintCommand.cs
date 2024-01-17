using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Lint.Handlers;
using KSail.Options;

namespace KSail.Commands.Lint;

internal sealed class KSailLintCommand : Command
{
  private readonly NameArgument nameArgument = new();
  private readonly ManifestsOption manifestsOption = new() { IsRequired = true };
  internal KSailLintCommand() : base(
   "lint", "Lint manifest files"
  )
  {
    AddArgument(nameArgument);
    AddOption(manifestsOption);
    this.SetHandler(KSailLintCommandHandler.HandleAsync, nameArgument, manifestsOption);
  }
}
