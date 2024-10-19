using System.CommandLine;
using KSail.Commands.Stop.Handlers;
using KSail.Options;

namespace KSail.Commands.Stop;

sealed class KSailStopCommand : Command
{
  readonly NameOption _nameOption = new();

  internal KSailStopCommand() : base("stop", "Stop a cluster")
  {
    AddOption(_nameOption);

    this.SetHandler(async (context) =>
    {
      var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
      config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));

      var handler = new KSailStopCommandHandler(config);
      try
      {
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
