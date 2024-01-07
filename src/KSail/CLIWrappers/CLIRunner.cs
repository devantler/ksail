using System.Text;
using CliWrap;
using CliWrap.EventStream;

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
        if (!silent)
        {
          Console.WriteLine(cmdEvent);
        }
        _ = result.AppendLine(cmdEvent.ToString());
      }
    }
    catch (Exception)
    {
      Console.WriteLine($"🚨 An error occurred while running '{command}'...");
      Environment.Exit(1);
    }
    return result.ToString();
  }
}
