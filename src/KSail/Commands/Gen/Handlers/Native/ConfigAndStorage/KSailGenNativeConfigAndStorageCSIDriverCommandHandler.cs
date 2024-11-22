
using Devantler.KubernetesGenerator.Native.ConfigAndStorage;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.ConfigAndStorage;

class KSailGenNativeConfigAndStorageCSIDriverCommandHandler
{
  readonly CSIDriverGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1CSIDriver
    {
      ApiVersion = "storage.k8s.io/v1",
      Kind = "CSIDriver",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      Spec = new V1CSIDriverSpec()
      {
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
