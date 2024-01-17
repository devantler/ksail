using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Init.Handlers;
using KSail.Options;

namespace KSail.Commands.Init;

sealed class KSailInitCommand : Command
{
  readonly NameArgument nameArgument = new();
  readonly ManifestsOption manifestsOption = new() { IsRequired = true };
  public KSailInitCommand() : base("init", "Initializes a new K8s GitOps project.")
  {
    AddArgument(nameArgument);
    AddOption(manifestsOption);

    this.SetHandler(KSailInitCommandHandler.HandleAsync, nameArgument, manifestsOption);
  }
}
