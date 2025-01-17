
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Workloads;
using KSail.Commands.Gen.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Native.Workloads;

class KSailGenNativeWorkloadsJobCommand : Command
{
  readonly FileOutputOption _outputOption = new("./job.yaml");
  readonly KSailGenNativeWorkloadsJobCommandHandler _handler = new();
  public KSailGenNativeWorkloadsJobCommand() : base("job", "Generate a 'batch/v1/Job' resource.")
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
        catch (OperationCanceledException ex)
        {
          ExceptionHandler.HandleException(ex);
          context.ExitCode = 1;
        }
      }
    );
  }
}
