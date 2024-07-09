using System.Text;
using CliWrap;
using CliWrap.EventStream;
using Spectre.Console;

namespace KSail.CLIWrappers;

class CLIRunner()
{
  public static async Task<(int ExitCode, string Result)> RunAsync(Command command, CancellationToken cancellationToken, CommandResultValidation validation = CommandResultValidation.ZeroExitCode, bool silent = false)
  {
    bool isFaulty = false;
    StringBuilder result = new();
    try
    {
      await foreach (var cmdEvent in command.WithValidation(validation).ListenAsync(cancellationToken: cancellationToken))
      {
        switch (cmdEvent)
        {
          case StartedCommandEvent started:
            if (System.Diagnostics.Debugger.IsAttached)
            {
              AnsiConsole.MarkupLine($"[bold blue]DEBUG[/] Process started: {started.ProcessId}");
            }
            break;
          case StandardOutputCommandEvent stdOut:
            if (!silent)
            {
              Console.WriteLine(stdOut.Text);
            }
            _ = result.AppendLine(stdOut.Text);
            break;
          case StandardErrorCommandEvent stdErr:
            if (!silent)
            {
              AnsiConsole.MarkupLine(stdErr.Text);
            }
            _ = result.AppendLine(stdErr.Text);
            break;
          case ExitedCommandEvent exited:
            if (System.Diagnostics.Debugger.IsAttached)
            {
              AnsiConsole.MarkupLine($"[bold blue]DEBUG[/] Process exited with code {exited.ExitCode}");
            }
            break;
          default:
            if (cmdEvent is null)
            {
              Console.WriteLine("✕ Command event is 'null'");
              return (1, "");
            }
            break;
        }
      }
    }
    catch
    {
      isFaulty = true;
    }
    return isFaulty ? (1, "") : (0, result.ToString());
  }
}
