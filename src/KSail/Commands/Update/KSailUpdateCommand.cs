using System.CommandLine;
using KSail.Commands.Update.Handlers;
using KSail.Options;

namespace KSail.Commands.Update;

sealed class KSailUpdateCommand : Command
{
  readonly NameOption nameOption = new("The name of the cluster to update manifests for") { IsRequired = true };
  readonly ManifestsOption manifestsOption = new() { IsRequired = true };
  internal KSailUpdateCommand() : base(
    "update",
    "Update manifests in an OCI registry"
  )
  {
    AddOption(nameOption);
    AddOption(manifestsOption);
    this.SetHandler(KSailUpdateCommandHandler.HandleAsync, nameOption, manifestsOption);
  }
}
