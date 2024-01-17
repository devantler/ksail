using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Update.Handlers;
using KSail.Options;

namespace KSail.Commands.Update;

internal sealed class KSailUpdateCommand : Command
{
  private readonly NameArgument nameArgument = new();
  private readonly ManifestsOption manifestsOption = new() { IsRequired = true };
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
