using System.CommandLine;
using System.Text;
using CliWrap;
using CliWrap.EventStream;
using CliWrap.Exceptions;

namespace KSail.CLIWrappers;

class CLIRunner(IConsole console)
{
  readonly IConsole console = console;
  public async Task<string> RunAsync(CliWrap.Command command, CommandResultValidation validation = CommandResultValidation.ZeroExitCode, bool silent = false)
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
            console.WriteLine(cmdEvent.ToString() ?? "");
          }
          _ = result.AppendLine(cmdEvent.ToString());
        }
      }
    }
    catch (CommandExecutionException e)
    {
      console.WriteLine($"🚨 An error occurred while running '{command}': {e.Message}...");
      Environment.Exit(1);
    }
    return result.ToString();
  }
}
