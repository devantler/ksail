using System.CommandLine;
using KSail.Commands.SOPS.Handlers;
using KSail.Commands.SOPS.Options;

namespace KSail.Commands.SOPS;

sealed class KSailSOPSCommand : Command
{
  readonly ShowPublicKeyOption _showPublicKeyOption = new();
  readonly ShowPrivateKeyOption _showPrivateKeyOption = new();
  internal KSailSOPSCommand() : base("sops", "manage SOPS GPG key")
  {
    AddOption(_showPublicKeyOption);
    AddOption(_showPrivateKeyOption);

    this.SetHandler(KSailSOPSCommandHandler.Handle, _showPublicKeyOption, _showPrivateKeyOption);
  }
}
