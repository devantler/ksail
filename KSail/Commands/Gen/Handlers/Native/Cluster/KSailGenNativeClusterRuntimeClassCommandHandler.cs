using Devantler.KubernetesGenerator.Native.Cluster;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Cluster;

class KSailGenNativeClusterRuntimeClassCommandHandler
{
  readonly RuntimeClassGenerator _generator = new();

  internal async Task HandleAsync(string outputPath, CancellationToken cancellationToken = default)
  {
    var model = new V1RuntimeClass()
    {
      ApiVersion = "node.k8s.io/v1",
      Kind = "RuntimeClass",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      Handler = "<handler>"
    };
    await _generator.GenerateAsync(model, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
