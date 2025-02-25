
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Config;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Config;

class KSailGenConfigSOPSCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly GenericPathOption _outputOption = new("./.sops.yaml");
  readonly KSailGenConfigSOPSCommandHandler _handler = new();
  public KSailGenConfigSOPSCommand() : base("sops", "Generate a SOPS configuration file.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        try
        {
          string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
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
