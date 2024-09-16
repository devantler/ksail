
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Workloads;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Workloads;

class KSailGenNativeWorkloadsDeploymentCommand : Command
{
  readonly FileOutputOption _outputOption = new("./deployment.yaml");
  readonly KSailGenNativeWorkloadsDeploymentCommandHandler _handler = new();
  public KSailGenNativeWorkloadsDeploymentCommand() : base("deployment", "Generate a 'apps/v1/Deployment' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        string outputPath = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        await _handler.HandleAsync(outputPath, context.GetCancellationToken()).ConfigureAwait(false);
      }
    );
  }
}
