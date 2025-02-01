using Devantler.KubernetesGenerator.Native;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativeSecretCommandHandler
{
  readonly SecretGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1Secret
    {
      ApiVersion = "v1",
      Kind = "Secret",
      Metadata = new V1ObjectMeta()
      {
        Name = "my-secret",
        NamespaceProperty = "my-namespace"
      },
      Type = "Opaque",
      StringData = new Dictionary<string, string>()
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
