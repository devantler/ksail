using System.CommandLine;
using KSail.Commands.Stop.Handlers;
using KSail.Options;
using KSail.Utils;
using YamlDotNet.Core;

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
        Console.WriteLine($"🛑 Stopping cluster '{config.Spec.Project.Distribution.ToString().ToLower(System.Globalization.CultureInfo.CurrentCulture)}-{config.Metadata.Name}'");
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        if (context.ExitCode == 0)
        {
          Console.WriteLine("✔ Cluster stopped");
        }
        else
        {
          throw new KSailException("Cluster could not be stopped");
        }
      }
      catch (YamlException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (KSailException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
