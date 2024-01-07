using System.CommandLine;
using KSail.Commands.Update.Handlers;
using KSail.Options;

namespace KSail.Commands.Update;

sealed class UpdateCommand : Command
{
  readonly NameOption _nameOption = new("the name of the cluster to update manifests for");
  readonly ManifestsPathOption _manifestsPathOption = new();
  internal UpdateCommand() : base(
    "update",
    "update manifests in an OCI registry"
  )
  {
    AddOption(_nameOption);
    AddOption(_manifestsPathOption);
    this.SetHandler(KSailUpdateCommandHandler.Handle, _nameOption, _manifestsPathOption);
  }
}
