
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Config;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Config;

class KSailGenConfigKSailCommand : Command
{
  readonly FileOutputOption _outputOption = new("./ksail-config.yaml");
  readonly KSailGenKSailConfigCommandHandler _handler = new();
  public KSailGenConfigKSailCommand() : base("ksail", "Generate a 'ksail.io/v1alpha1/Cluster' resource.")
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
