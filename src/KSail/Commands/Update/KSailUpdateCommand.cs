using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Update.Handlers;
using KSail.Options;

namespace KSail.Commands.Update;

sealed class KSailUpdateCommand : Command
{
  readonly NameArgument nameArgument = new();
  readonly ManifestsOption manifestsOption = new() { IsRequired = true };
  internal KSailUpdateCommand() : base(
    "update",
    "Update manifests in an OCI registry"
  )
  {
    AddArgument(nameArgument);
    AddOption(manifestsOption);
    this.SetHandler(KSailUpdateCommandHandler.HandleAsync, nameArgument, manifestsOption);
  }
}
