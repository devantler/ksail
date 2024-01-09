using System.CommandLine;
using KSail.Commands.Update.Handlers;
using KSail.Options;

namespace KSail.Commands.Update;

sealed class KSailUpdateCommand : Command
{
  readonly NameOption nameOption = new("the name of the cluster to update manifests for") { IsRequired = true };
  readonly ManifestsPathOption manifestsPathOption = new() { IsRequired = true };
  internal KSailUpdateCommand() : base(
    "update",
    "update manifests in an OCI registry"
  )
  {
    AddOption(nameOption);
    AddOption(manifestsPathOption);
    this.SetHandler(KSailUpdateCommandHandler.HandleAsync, nameOption, manifestsPathOption);
  }
}
