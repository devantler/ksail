using Devantler.KubernetesGenerator.Native;
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
        Name = "my-config-map"
      },
      Data = new Dictionary<string, string>()
      {
        { "key1", "value1" },
        { "key2", "value2" }
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
