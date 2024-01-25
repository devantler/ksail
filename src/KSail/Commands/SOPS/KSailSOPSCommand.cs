using System.CommandLine;
using KSail.Commands.SOPS.Handlers;
using KSail.Commands.SOPS.Options;

namespace KSail.Commands.SOPS;

sealed class KSailSOPSCommand : Command
{
  readonly GenerateKeyOption generateKeyOption = new();
  readonly ShowPublicKeyOption showPublicKeyOption = new();
  readonly ShowPrivateKeyOption showPrivateKeyOption = new();
  readonly EncryptOption encryptOption = new();
  readonly DecryptOption decryptOption = new();
  readonly ImportOption importOption = new();
  readonly ExportOption exportOption = new();
  internal KSailSOPSCommand() : base("sops", "Manage SOPS key")
  {
    AddOption(generateKeyOption);
    AddOption(showPublicKeyOption);
    AddOption(showPrivateKeyOption);
    AddOption(encryptOption);
    AddOption(decryptOption);
    AddOption(importOption);
    AddOption(exportOption);

    AddValidator(result =>
    {
      if (result.Children.Count == 0)
      {
        result.ErrorMessage = "No option specified";
      }
      else if (result.Children.Count > 1)
      {
        result.ErrorMessage = "More than one option specified";
      }
    });

    this.SetHandler(KSailSOPSCommandHandler.HandleAsync, generateKeyOption, showPublicKeyOption, showPrivateKeyOption, encryptOption, decryptOption, importOption, exportOption);
  }
}
