using System.CommandLine;
using KSail.Commands.Start.Handlers;
using KSail.Options;

namespace KSail.Commands.Start;

sealed class KSailStartCommand : Command
{
  readonly NameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };

  internal KSailStartCommand() : base("start", "Start a cluster")
  {
    AddOption(_nameOption);

    this.SetHandler(async (context) =>
    {
      var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
      config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));

      var handler = new KSailStartCommandHandler(config);
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
