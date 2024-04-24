using System.Text;
using CliWrap;
using CliWrap.EventStream;

namespace KSail.CLIWrappers;

class CLIRunner()
{
  public static async Task<(int ExitCode, string Result)> RunAsync(Command command, CancellationToken cancellationToken, CommandResultValidation validation = CommandResultValidation.ZeroExitCode, bool silent = false)
  {
    StringBuilder result = new();
    try
    {
      await foreach (var cmdEvent in command.WithValidation(validation).ListenAsync(cancellationToken: cancellationToken))
      {
        if (cmdEvent is StandardOutputCommandEvent or StandardErrorCommandEvent)
        {
          if (cmdEvent is null)
          {
            Console.WriteLine("✕ Command event is 'null'");
            return (1, "");
          }
          if (!silent)
          {
            Console.WriteLine(cmdEvent.ToString() ?? "");
          }
          _ = result.AppendLine(cmdEvent.ToString());
        }
      }
    }
    catch
    {
    }
    return (0, result.ToString());
  }
}
