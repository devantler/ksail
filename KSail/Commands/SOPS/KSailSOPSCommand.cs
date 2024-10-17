using System.CommandLine;
using System.CommandLine.Parsing;
using KSail.Commands.SOPS.Handlers;
using KSail.Commands.SOPS.Options;
using KSail.Options;

namespace KSail.Commands.SOPS;

sealed class KSailSOPSCommand : Command
{
  readonly NameOption _clusterNameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly GenerateKeyOption _generateKeyOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ShowKeyOption _showKeyOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ShowPublicKeyOption _showPublicKeyOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ShowPrivateKeyOption _showPrivateKeyOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly EncryptOption _encryptOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly DecryptOption _decryptOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ImportOption _importOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ExportOption _exportOption = new() { Arity = ArgumentArity.ZeroOrOne };
  internal KSailSOPSCommand() : base("sops", "Manage secrets with SOPS")
  {
    AddOption(_clusterNameOption);
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
      string clusterName = context.ParseResult.GetValueForOption(_clusterNameOption) ?? throw new InvalidOperationException("Cluster name is required");
      bool generateKey = context.ParseResult.GetValueForOption(_generateKeyOption);
      bool showKey = context.ParseResult.GetValueForOption(_showKeyOption);
      bool showPublicKey = context.ParseResult.GetValueForOption(_showPublicKeyOption);
      bool showPrivateKey = context.ParseResult.GetValueForOption(_showPrivateKeyOption);
      string encrypt = context.ParseResult.GetValueForOption(_encryptOption) ?? "";
      string decrypt = context.ParseResult.GetValueForOption(_decryptOption) ?? "";
      string import = context.ParseResult.GetValueForOption(_importOption) ?? "";
      string export = context.ParseResult.GetValueForOption(_exportOption) ?? "";

      var cancellationToken = context.GetCancellationToken();
      try
      {
        var handler = new KSailSOPSCommandHandler();
        context.ExitCode = await handler.HandleAsync(clusterName, generateKey, showKey, showPublicKey, showPrivateKey, encrypt, decrypt, import, export, cancellationToken).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
