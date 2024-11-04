
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Config;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Config;

class KSailGenConfigSOPSCommand : Command
{
  readonly FileOutputOption _outputOption = new("./.sops.yaml");
  readonly KSailGenConfigSOPSCommandHandler _handler = new();
  public KSailGenConfigSOPSCommand() : base("sops", "Generate a SOPS configuration file.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        try
        {
          string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
          Console.WriteLine($"✚ Generating {outputFile}");
          context.ExitCode = await _handler.HandleAsync(outputFile, context.GetCancellationToken()).ConfigureAwait(false);
        }
        catch (OperationCanceledException ex)
        {
          ExceptionHandler.HandleException(ex);
          context.ExitCode = 1;
        }
      }
    );
  }
}
