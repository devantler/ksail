// using System.CommandLine;
// using KSail.Options;
// using KSail.Utils;

// namespace KSail.Commands.SOPS.Commands;

// sealed class KSailSOPSDecryptCommand : Command
// {
//   readonly PathOption _pathOption = new("File to decrypt");
//   internal KSailSOPSDecryptCommand() : base("decrypt", "Decrypt a file")
//   {
//     AddOption(_pathOption);

//     this.SetHandler(async (context) =>
//     {
//       try
//       {
//         var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);

//         var cancellationToken = context.GetCancellationToken();
//         //var handler = new KSailSOPSListCommandHandler(config);

//         //_ = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
//       }
//       catch (OperationCanceledException ex)
//       {
//         _ = _exceptionHandler.HandleException(ex);
//         context.ExitCode = 1;
//       }
//     });
//   }
// }
