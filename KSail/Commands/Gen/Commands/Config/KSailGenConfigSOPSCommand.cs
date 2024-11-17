
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Config;
using KSail.Commands.Gen.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Config;

class KSailGenConfigSopsCommand : Command
{
  readonly FileOutputOption _outputOption = new("./.sops.yaml");
  readonly KSailGenConfigSopsCommandHandler _handler = new();
  public KSailGenConfigSopsCommand() : base("sops", "Generate a Sops configuration file.")
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
        catch (OperationCanceledException ex)
        {
          ExceptionHandler.HandleException(ex);
          context.ExitCode = 1;
        }
      }
    );
  }
}
