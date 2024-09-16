using Devantler.KubernetesGenerator.Native.ConfigAndStorage;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.ConfigAndStorage;

class KSailGenNativeConfigAndStorageSecretCommandHandler
{
  readonly SecretGenerator _generator = new();
  internal async Task HandleAsync(string outputPath, CancellationToken cancellationToken = default)
  {
    var model = new V1Secret
    {
      ApiVersion = "v1",
      Kind = "Secret",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
        NamespaceProperty = "<namespace>"
      },
      Type = "<type>",
      StringData = new Dictionary<string, string>()
    };
    await _generator.GenerateAsync(model, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
