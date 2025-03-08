using Devantler.KubernetesGenerator.Native;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativeNamespaceCommandHandler(string outputFile, bool overwrite)
{
  readonly NamespaceGenerator _generator = new();
  internal async Task<int> HandleAsync(CancellationToken cancellationToken = default)
  {
    var model = new V1Namespace()
    {
      ApiVersion = "v1",
      Kind = "Namespace",
      Metadata = new V1ObjectMeta()
      {
        Name = "my-namespace"
      }
    };
    await _generator.GenerateAsync(model, outputFile, overwrite, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
