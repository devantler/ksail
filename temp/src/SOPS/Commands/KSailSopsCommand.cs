// using System.CommandLine;
// using KSail.Commands.SOPS.Options;
// using KSail.Options;

// namespace KSail.Commands.SOPS;

// sealed class KSailSOPSCommand : Command
// {
//   readonly NameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
//   readonly GenerateKeyOption _generateKeyOption = new() { Arity = ArgumentArity.ZeroOrOne };
//   readonly EncryptOption _encryptOption = new() { Arity = ArgumentArity.ZeroOrOne };
//   readonly DecryptOption _decryptOption = new() { Arity = ArgumentArity.ZeroOrOne };
//   internal KSailSOPSCommand() : base("sops", "Manage secrets with SOPS")
//   {
//     AddArgument(_generateKeyOption);

//     AddArgument(_showKeyOption);
//     AddOption(_showPublicKeyOption);
//     AddOption(_showPrivateKeyOption);
//     AddOption(_nameOption);

//     AddArgument(_editOption);
//     AddArgument(_encryptOption);
//     AddArgument(_decryptOption);
//     AddArgument(_importOption);
//     AddArgument(_exportOption);

//     this.SetHandler(async (context) =>
//     {
//       string clusterName = context.ParseResult.GetValueForOption(_nameOption)!;
//       bool generateKey = context.ParseResult.GetValueForOption(_generateKeyOption);
//       bool showKey = context.ParseResult.GetValueForOption(_showKeyOption);
//       bool showPublicKey = context.ParseResult.GetValueForOption(_showPublicKeyOption);
//       bool showPrivateKey = context.ParseResult.GetValueForOption(_showPrivateKeyOption);
//       string encrypt = context.ParseResult.GetValueForOption(_encryptOption) ?? "";
//       string decrypt = context.ParseResult.GetValueForOption(_decryptOption) ?? "";
//       string import = context.ParseResult.GetValueForOption(_importOption) ?? "";
//       string export = context.ParseResult.GetValueForOption(_exportOption) ?? "";

//       var cancellationToken = context.GetCancellationToken();
//       try
//       {
//         var handler = new KSailSOPSCommandHandler();
//         context.ExitCode = await handler.HandleAsync(clusterName, generateKey, showKey, showPublicKey, showPrivateKey, encrypt, decrypt, import, export, cancellationToken).ConfigureAwait(false);
//       }
//       catch (OperationCanceledException)
//       {
//         context.ExitCode = 1;
//       }
//     });
//   }
// }
