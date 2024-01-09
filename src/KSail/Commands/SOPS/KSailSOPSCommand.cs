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

    this.SetHandler(async (showPublicKey, showPrivateKey) =>
    {
      if (!showPublicKey && !showPrivateKey)
      {
        Console.WriteLine($"❌ Either '{_showPublicKeyOption.Aliases.First()}' or '{_showPrivateKeyOption.Aliases.First()}' must be specified to list SOPS GPG keys...");
        Environment.Exit(1);
      }
      await KSailSOPSCommandHandler.HandleAsync(showPublicKey, showPrivateKey);
    }, _showPublicKeyOption, _showPrivateKeyOption);
  }
}
