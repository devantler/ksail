// using System.CommandLine;
// using KSail.Options;
// using KSail.Utils;

// namespace KSail.Commands.SOPS.Commands;

// sealed class KSailSOPSImportCommand : Command
// {
//   readonly PathOption _pathOption = new("Path to import key from");
//   internal KSailSOPSImportCommand() : base("import", "Import a key")
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
//         ExceptionHandler.HandleException(ex);
//         context.ExitCode = 1;
//       }
//     });
//   }
// }
