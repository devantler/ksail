
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Config;
using KSail.Commands.Gen.Options;
using KSail.Deserializer;

namespace KSail.Commands.Gen.Commands.Config;

class KSailGenConfigK3dCommand : Command
{
  readonly KSailClusterDeserializer _deserializer = new();
  readonly FileOutputOption _outputOption = new("./k3d-config.yaml");
  public KSailGenConfigK3dCommand() : base("k3d", "Generate a 'k3d.io/v1alpha5/Simple' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        var config = await _deserializer.LocateAndDeserializeAsync().ConfigureAwait(false);
        await config.SetConfigValueAsync("Spec.ManifestDirectory", context.ParseResult.RootCommandResult.GetValueForOption(_outputOption)).ConfigureAwait(false);
        var handler = new KSailGenConfigK3dCommandHandler(config);
        try
        {
          await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
          context.ExitCode = 1;
        }
      }
    );
  }
}
