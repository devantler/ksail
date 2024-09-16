
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.ConfigAndStorage;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.ConfigAndStorage;

class KSailGenNativeConfigAndStorageSecretCommand : Command
{
  readonly FileOutputOption _outputOption = new("./secret.yaml");
  readonly KSailGenNativeConfigAndStorageSecretCommandHandler _handler = new();
  public KSailGenNativeConfigAndStorageSecretCommand() : base("secret", "Generate a 'core/v1/Secret' resource.")
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
