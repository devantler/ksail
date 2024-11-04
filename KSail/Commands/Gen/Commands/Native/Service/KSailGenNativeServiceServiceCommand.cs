
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Services;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Service;

class KSailGenNativeServiceServiceCommand : Command
{
  readonly FileOutputOption _outputOption = new("./service.yaml");
  readonly KSailGenNativeServiceServiceCommandHandler _handler = new();
  public KSailGenNativeServiceServiceCommand() : base("service", "Generate a 'core/v1/Service' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        try
        {
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
