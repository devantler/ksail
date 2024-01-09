using System.Text;
using CliWrap;
using CliWrap.EventStream;
using CliWrap.Exceptions;

namespace KSail.CLIWrappers;

static class CLIRunner
{
  public static async Task<string> RunAsync(Command command, CommandResultValidation validation = CommandResultValidation.ZeroExitCode, bool silent = false)
  {
    StringBuilder result = new();
    try
    {
      await foreach (var cmdEvent in command.WithValidation(validation).ListenAsync())
      {
        if (cmdEvent is StandardOutputCommandEvent or StandardErrorCommandEvent)
        {
          if (!silent)
          {
            Console.WriteLine(cmdEvent);
          }
          _ = result.AppendLine(cmdEvent.ToString());
        }
      }
    }
    catch (CommandExecutionException e)
    {
      Console.WriteLine($"🚨 An error occurred while running '{command}': {e.Message}...");
      Environment.Exit(1);
    }
    return result.ToString();
  }
}
