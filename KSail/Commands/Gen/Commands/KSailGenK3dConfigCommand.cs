
using System.CommandLine;
using KSail.Commands.Gen.Handlers;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands;

class KSailGenK3dConfigCommand : Command
{
  readonly FileOutputOption _outputOption = new() { IsRequired = true };
  readonly KSailGenK3dConfigCommandHandler _handler = new();
  public KSailGenK3dConfigCommand() : base("k3d-config", "Generate a K3d configuration file.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        string outputPath = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        context.ExitCode = await _handler.HandleAsync(outputPath, context.GetCancellationToken()).ConfigureAwait(false);
      }
    );
  }
}
