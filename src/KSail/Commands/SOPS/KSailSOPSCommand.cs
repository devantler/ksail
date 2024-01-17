using System.CommandLine;
using KSail.Commands.SOPS.Handlers;
using KSail.Commands.SOPS.Options;

namespace KSail.Commands.SOPS;

internal sealed class KSailSOPSCommand : Command
{
  private readonly GenerateKeyOption generateKeyOption = new();
  private readonly ShowPublicKeyOption showPublicKeyOption = new();
  private readonly ShowPrivateKeyOption showPrivateKeyOption = new();
  private readonly EncryptOption encryptOption = new();
  private readonly DecryptOption decryptOption = new();
  internal KSailSOPSCommand() : base("sops", "Manage SOPS key")
  {
    AddOption(generateKeyOption);
    AddOption(showPublicKeyOption);
    AddOption(showPrivateKeyOption);
    AddOption(encryptOption);
    AddOption(decryptOption);

    AddValidator(result =>
    {
      if (result.Children.Count == 0)
      {
        result.ErrorMessage = "You must specify either --generate-key, --show-public-key, --show-private-key, --encrypt or --decrypt";
      }
      else if (result.Children.Count > 1)
      {
        result.ErrorMessage = "You must specify only one of --generate-key, --show-public-key, --show-private-key, --encrypt or --decrypt";
      }
    });

    this.SetHandler(KSailSOPSCommandHandler.HandleAsync, generateKeyOption, showPublicKeyOption, showPrivateKeyOption, encryptOption, decryptOption);
  }
}
