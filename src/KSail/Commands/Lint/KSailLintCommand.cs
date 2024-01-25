using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Lint.Handlers;
using KSail.Options;

namespace KSail.Commands.Lint;

sealed class KSailLintCommand : Command
{
  readonly NameArgument nameArgument = new() { Arity = ArgumentArity.ExactlyOne };
  readonly ManifestsOption manifestsOption = new() { IsRequired = true };
  internal KSailLintCommand() : base(
    "lint", "Lint manifest files"
  )
  {
    AddArgument(nameArgument);
    AddOption(manifestsOption);
    this.SetHandler(KSailLintCommandHandler.HandleAsync, nameArgument, manifestsOption);
  }
}
