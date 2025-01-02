using Devantler.KubernetesGenerator.Native.ConfigAndStorage;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativeConfigMapCommandHandler
{
  readonly ConfigMapGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1ConfigMap()
    {
      ApiVersion = "v1",
      Kind = "ConfigMap",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      Data = new Dictionary<string, string>()
      {
        { "<key>", "<value>" }
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
