using System.CommandLine;
using KSail.Commands.Start.Handlers;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Start;

sealed class KSailStartCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly MetadataNameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ProjectEngineOption _engineOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ProjectDistributionOption _distributionOption = new() { Arity = ArgumentArity.ZeroOrOne };

  internal KSailStartCommand() : base("start", "Start a cluster")
  {
    AddOptions();

    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync(name: context.ParseResult.GetValueForOption(_nameOption)).ConfigureAwait(false);
        config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));
        config.UpdateConfig("Spec.Project.Engine", context.ParseResult.GetValueForOption(_engineOption));
        config.UpdateConfig("Spec.Project.Distribution", context.ParseResult.GetValueForOption(_distributionOption));

        Console.WriteLine($"🟢 Starting cluster '{config.Spec.Connection.Context}'");
        var handler = new KSailStartCommandHandler(config);
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        if (context.ExitCode == 0)
        {
          Console.WriteLine("✔ Cluster started");
        }
        else
        {
          throw new KSailException("Cluster could not be started");
        }
      }
      catch (Exception ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }

  void AddOptions()
  {
    AddOption(_nameOption);
    AddOption(_engineOption);
    AddOption(_distributionOption);
  }
}
