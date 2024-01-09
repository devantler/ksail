using System.CommandLine;
using System.CommandLine.Parsing;
using System.Linq;
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

    AddValidator(result =>
    {
      if (!result.GetValueForOption(_showPublicKeyOption) && !result.GetValueForOption(_showPrivateKeyOption))
      {
        Console.WriteLine($"❌ Either '{_showPublicKeyOption.Aliases.First()}' or '{_showPrivateKeyOption.Aliases.First()}' must be specified to list SOPS GPG keys...");
        Environment.Exit(1);
      }
    });

    this.SetHandler(KSailSOPSCommandHandler.HandleAsync, _showPublicKeyOption, _showPrivateKeyOption);
  }
}
