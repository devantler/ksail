
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterResourceQuotaCommand : Command
{
  readonly FileOutputOption _outputOption = new("./resource-quota.yaml");
  readonly KSailGenNativeClusterResourceQuotaCommandHandler _handler = new();
  public KSailGenNativeClusterResourceQuotaCommand() : base("resource-quota", "Generate a 'core/v1/ResourceQuota' resource.")
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
