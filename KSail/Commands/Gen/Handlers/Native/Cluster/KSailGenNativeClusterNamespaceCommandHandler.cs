using Devantler.KubernetesGenerator.Native.Cluster;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Cluster;

class KSailGenNativeClusterNamespaceCommandHandler
{
  readonly NamespaceGenerator _generator = new();
  internal async Task HandleAsync(string outputPath, CancellationToken cancellationToken)
  {
    var model = new V1Namespace()
    {
      ApiVersion = "v1",
      Kind = "Namespace",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      }
    };
    await _generator.GenerateAsync(model, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
