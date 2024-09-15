using Devantler.KubernetesGenerator.Native.Metadata;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Metadata;

class KSailGenNativeMetadataLimitRangeCommandHandler
{
  readonly LimitRangeGenerator _generator = new();
  internal async Task HandleAsync(string outputPath, CancellationToken cancellationToken = default)
  {
    var model = new V1LimitRange()
    {
      ApiVersion = "v1",
      Kind = "LimitRange",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>"
      },
      Spec = new V1LimitRangeSpec()
      {
        Limits = []
      }
    };
    await _generator.GenerateAsync(model, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);

  }
}
