using System.CommandLine;
using KSail.Commands.List.Handlers;
using KSail.Commands.List.Options;
using KSail.Utils;
using YamlDotNet.Core;

namespace KSail.Commands.List;

sealed class KSailListCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly AllOption _allOption = new() { Arity = ArgumentArity.ZeroOrOne };
  internal KSailListCommand() : base("list", "List active clusters")
  {
    AddOption(_allOption);
    this.SetHandler(async (context) =>
    {
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
        config.UpdateConfig("Spec.CLI.List.All", context.ParseResult.GetValueForOption(_allOption));
        var cancellationToken = context.GetCancellationToken();
        var handler = new KSailListCommandHandler(config);

        Console.WriteLine("📋 Listing clusters");
        _ = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        Console.WriteLine();
      }
      catch (YamlException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (OperationCanceledException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }
}
