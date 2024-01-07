using CliWrap;
using CliWrap.EventStream;

namespace KSail.CLIWrappers;

static class CLIRunner
{
  public static async Task<string> RunAsync(Command command, CommandResultValidation validation = CommandResultValidation.ZeroExitCode)
  {
    string result = "";
    try
    {
      await foreach (var cmdEvent in command.WithValidation(validation).ListenAsync())
      {
        Console.WriteLine(cmdEvent);
        result += cmdEvent;
      }
    }
    catch (Exception)
    {
      Environment.Exit(1);
    }
    return result;
  }
}
