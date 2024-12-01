// using System.CommandLine;
// using KSail.Utils;

// namespace KSail.Commands.SOPS.Commands;

// sealed class KSailSOPSRemoveCommand : Command
// {
//   internal KSailSOPSRemoveCommand() : base("rm", "Remove a key")
//   {
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
//         ExceptionHandler.HandleException(ex);
//         context.ExitCode = 1;
//       }
//     });
//   }
// }
