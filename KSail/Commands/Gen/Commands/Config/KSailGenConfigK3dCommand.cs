
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Config;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Config;

class KSailGenConfigK3dCommand : Command
{
  readonly FileOutputOption _outputOption = new("./k3d-config.yaml");
  readonly KSailGenK3dConfigCommandHandler _handler = new();
  public KSailGenConfigK3dCommand() : base("k3d", "Generate a 'k3d.io/v1alpha5/Simple' resource.")
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
