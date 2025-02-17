
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Native;

class KSailGenNativePriorityClassCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly OutputOption _outputOption = new("./priority-class.yaml");
  readonly KSailGenNativePriorityClassCommandHandler _handler = new();

  public KSailGenNativePriorityClassCommand() : base("priority-class", "Generate a 'scheduling.k8s.io/v1/PriorityClass' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        try
        {
          Console.WriteLine($"✚ generating {outputFile}");
          context.ExitCode = await _handler.HandleAsync(outputFile, context.GetCancellationToken()).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
          _ = _exceptionHandler.HandleException(ex);
          context.ExitCode = 1;
        }
      }
    );
  }
}
