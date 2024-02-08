using System.CommandLine;
using System.CommandLine.Parsing;
using KSail.Arguments;
using KSail.Commands.SOPS.Handlers;
using KSail.Commands.SOPS.Options;

namespace KSail.Commands.SOPS;

sealed class KSailSOPSCommand : Command
{
  readonly ClusterNameArgument _clusterNameArgument = new();
  readonly GenerateKeyOption _generateKeyOption = new();
  readonly ShowKeyOption _showKeyOption = new();
  readonly ShowPublicKeyOption _showPublicKeyOption = new();
  readonly ShowPrivateKeyOption _showPrivateKeyOption = new();
  readonly EncryptOption _encryptOption = new();
  readonly DecryptOption _decryptOption = new();
  readonly ImportOption _importOption = new();
  readonly ExportOption _exportOption = new();
  internal KSailSOPSCommand() : base("sops", "Manage SOPS key")
  {
    AddArgument(_clusterNameArgument);
    AddOption(_generateKeyOption);
    AddOption(_showKeyOption);
    AddOption(_showPublicKeyOption);
    AddOption(_showPrivateKeyOption);
    AddOption(_encryptOption);
    AddOption(_decryptOption);
    AddOption(_importOption);
    AddOption(_exportOption);

    AddValidator(result =>
    {
      if (!result.Children.OfType<OptionResult>().Any())
      {
        result.ErrorMessage = "No option specified";
      }
      else if (result.Children.OfType<OptionResult>().Count() > 1)
      {
        result.ErrorMessage = "More than one option specified";
      }
    });

    this.SetHandler(async (context) =>
    {
      string clusterName = context.ParseResult.GetValueForArgument(_clusterNameArgument);
      bool generateKey = context.ParseResult.GetValueForOption(_generateKeyOption);
      bool showKey = context.ParseResult.GetValueForOption(_showKeyOption);
      bool showPublicKey = context.ParseResult.GetValueForOption(_showPublicKeyOption);
      bool showPrivateKey = context.ParseResult.GetValueForOption(_showPrivateKeyOption);
      string encrypt = context.ParseResult.GetValueForOption(_encryptOption) ?? "";
      string decrypt = context.ParseResult.GetValueForOption(_decryptOption) ?? "";
      string import = context.ParseResult.GetValueForOption(_importOption) ?? "";
      string export = context.ParseResult.GetValueForOption(_exportOption) ?? "";

      var token = context.GetCancellationToken();
      try
      {
        var handler = new KSailSOPSCommandHandler();
        context.ExitCode = await handler.HandleAsync(clusterName, generateKey, showKey, showPublicKey, showPrivateKey, encrypt, decrypt, import, export, token);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
