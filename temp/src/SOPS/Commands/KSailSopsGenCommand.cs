// using System.CommandLine;
// using Devantler.KeyManager.Core;
// using Devantler.KeyManager.Local.Age;
// using Devantler.Keys.Age;
// using KSail.Models;
// using KSail.Utils;

// namespace KSail.Commands.SOPS.Commands;

// sealed class KSailSOPSGenCommand : Command
// {
//   internal KSailSOPSGenCommand() : base("gen", "Generate a key")
//   {
//     this.SetHandler(async (context) =>
//     {
//       try
//       {
//         var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);

//         var cancellationToken = context.GetCancellationToken();
//         var handler = new KSailSOPGenCommandHandler(config);

//         Console.WriteLine("🔑 Generating key");
//         context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
//         Console.WriteLine();
//       }
//       catch (OperationCanceledException ex)
//       {
//         _ = _exceptionHandler.HandleException(ex);
//         context.ExitCode = 1;
//       }
//     });
//   }
// }

// class KSailSOPGenCommandHandler
// {
//   private KSailCluster _config;
//   readonly ILocalKeyManager<AgeKey> _keyManager = new LocalAgeKeyManager();

//   public KSailSOPGenCommandHandler(KSailCluster config) => _config = config;

//   internal async Task<int> HandleAsync(CancellationToken cancellationToken)
//   {
//     var key = await _keyManager.CreateKeyAsync(cancellationToken).ConfigureAwait(false);
//     Console.WriteLine(key);
//   }
// }
