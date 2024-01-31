using System.CommandLine;
using KSail.Commands.SOPS.Handlers;
using KSail.Commands.SOPS.Options;

namespace KSail.Commands.SOPS;

sealed class KSailSOPSCommand : Command
{
  readonly GenerateKeyOption _generateKeyOption = new();
  readonly ShowPublicKeyOption _showPublicKeyOption = new();
  readonly ShowPrivateKeyOption _showPrivateKeyOption = new();
  readonly EncryptOption _encryptOption = new();
  readonly DecryptOption _decryptOption = new();
  readonly ImportOption _importOption = new();
  readonly ExportOption _exportOption = new();
  internal KSailSOPSCommand() : base("sops", "Manage SOPS key")
  {
    AddOption(_generateKeyOption);
    AddOption(_showPublicKeyOption);
    AddOption(_showPrivateKeyOption);
    AddOption(_encryptOption);
    AddOption(_decryptOption);
    AddOption(_importOption);
    AddOption(_exportOption);

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

    this.SetHandler(async (context) =>
    {
      bool generateKey = context.ParseResult.GetValueForOption(_generateKeyOption);
      bool showPublicKey = context.ParseResult.GetValueForOption(_showPublicKeyOption);
      bool showPrivateKey = context.ParseResult.GetValueForOption(_showPrivateKeyOption);
      string encrypt = context.ParseResult.GetValueForOption(_encryptOption) ?? "";
      string decrypt = context.ParseResult.GetValueForOption(_decryptOption) ?? "";
      string import = context.ParseResult.GetValueForOption(_importOption) ?? "";
      string export = context.ParseResult.GetValueForOption(_exportOption) ?? "";

      var token = context.GetCancellationToken();
      try
      {
        context.ExitCode = await KSailSOPSCommandHandler.HandleAsync(generateKey, showPublicKey, showPrivateKey, encrypt, decrypt, import, export, token);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
