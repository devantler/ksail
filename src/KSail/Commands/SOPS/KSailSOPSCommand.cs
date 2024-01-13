using System.CommandLine;
using KSail.Commands.SOPS.Handlers;
using KSail.Commands.SOPS.Options;

namespace KSail.Commands.SOPS;

sealed class KSailSOPSCommand : Command
{
  readonly ShowPublicKeyOption showPublicKeyOption = new();
  readonly ShowPrivateKeyOption showPrivateKeyOption = new();
  internal KSailSOPSCommand() : base("sops", "Manage SOPS key")
  {
    AddOption(showPublicKeyOption);
    AddOption(showPrivateKeyOption);

    AddValidator(result =>
    {
      if (!result.GetValueForOption(showPublicKeyOption) && !result.GetValueForOption(showPrivateKeyOption))
      {
        Console.WriteLine($"✕ Either '{showPublicKeyOption.Aliases.First()}' or '{showPrivateKeyOption.Aliases.First()}' must be specified to list SOPS keys...");
        Environment.Exit(1);
      }
    });

    this.SetHandler(KSailSOPSCommandHandler.HandleAsync, showPublicKeyOption, showPrivateKeyOption);
  }
}
