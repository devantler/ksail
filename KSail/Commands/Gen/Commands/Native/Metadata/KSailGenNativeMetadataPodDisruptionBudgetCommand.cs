
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Metadata;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataPodDisruptionBudgetCommand : Command
{
  readonly FileOutputOption _outputOption = new("./pod-disruption-budget.yaml");
  readonly KSailGenNativeMetadataPodDisruptionBudgetCommandHandler _handler = new();

  public KSailGenNativeMetadataPodDisruptionBudgetCommand() : base("pod-disruption-budget", "Generate a 'policy/v1/PodDisruptionBudget' resource.")
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
