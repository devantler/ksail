
using Devantler.KubernetesGenerator.Native.ConfigAndStorage;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativeVolumeAttributesClassCommandHandler
{
  readonly VolumeAttributesClassGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1beta1VolumeAttributesClass
    {
      ApiVersion = "storage.k8s.io/v1beta1",
      Kind = "VolumeAttributesClass",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      DriverName = "<driverName>",
      Parameters = new Dictionary<string, string>()
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;

  }
}
