using System.Text;
using CliWrap;
using CliWrap.EventStream;
using CliWrap.Exceptions;

namespace KSail.CLIWrappers;

class CLIRunner()
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
          if (cmdEvent is null)
          {
            throw new InvalidOperationException("🚨 Command event is 'null'");
          }
          if (!silent)
          {
            Console.WriteLine(cmdEvent.ToString() ?? "");
          }
          _ = result.AppendLine(cmdEvent.ToString());
        }
      }
    }
    catch (CommandExecutionException e)
    {
      throw new InvalidOperationException($"🚨 An error occurred while running '{command}': {e.Message}...");
    }
    return result.ToString();
  }
}
