
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterAPIServiceCommand : Command
{
  readonly FileOutputOption _outputOption = new("./api-service.yaml");
  readonly KSailGenNativeClusterAPIServiceCommandHandler _handler = new();
  public KSailGenNativeClusterAPIServiceCommand() : base("api-service", "Generate a 'apiregistration.k8s.io/v1/APIService' resource.")
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
