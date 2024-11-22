using System.CommandLine;
using KSail.Commands.Stop.Handlers;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Stop;

sealed class KSailStopCommand : Command
{
  readonly NameOption _nameOption = new();

  internal KSailStopCommand() : base("stop", "Stop a cluster")
  {
    AddOption(_nameOption);

    this.SetHandler(async (context) =>
    {
      var config = await KSailClusterConfigLoader.LoadAsync(name: context.ParseResult.GetValueForOption(_nameOption)).ConfigureAwait(false);
      config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));

      var handler = new KSailStopCommandHandler(config);
      try
      {
        Console.WriteLine("🛑 Stopping cluster");
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        if (context.ExitCode == 0)
        {
          Console.WriteLine("👋 Cluster stopped");
        }
        else
        {
          Console.WriteLine("❌ Cluster could not be stopped");
        }
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
