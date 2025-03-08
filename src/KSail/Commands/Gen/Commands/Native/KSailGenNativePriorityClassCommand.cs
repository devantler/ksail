
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Native;

class KSailGenNativePriorityClassCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly GenericPathOption _outputOption = new("./priority-class.yaml");

  public KSailGenNativePriorityClassCommand() : base("priority-class", "Generate a 'scheduling.k8s.io/v1/PriorityClass' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        try
        {
          string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
          bool overwrite = context.ParseResult.RootCommandResult.GetValueForOption(CLIOptions.Generator.OverwriteOption) ?? false;
          if (overwrite)
          {
            Console.WriteLine($"✚ overwriting {outputFile}");
          }
          else
          {
            Console.WriteLine($"✚ generating {outputFile}");
          }
          KSailGenNativePriorityClassCommandHandler handler = new(outputFile, overwrite);
          context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
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
