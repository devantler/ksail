using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Init.Handlers;
using KSail.Options;

namespace KSail.Commands.Init;

sealed class KSailInitCommand : Command
{
  readonly NameArgument _nameArgument = new();
  readonly ManifestsOption _manifestsOption = new() { IsRequired = true };
  public KSailInitCommand() : base("init", "Initialize a new K8s cluster")
  {
    AddArgument(_nameArgument);
    AddOption(_manifestsOption);

    this.SetHandler(KSailInitCommandHandler.HandleAsync, _nameArgument, _manifestsOption);
  }
}
