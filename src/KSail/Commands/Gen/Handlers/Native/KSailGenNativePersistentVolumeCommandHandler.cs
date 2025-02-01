using Devantler.KubernetesGenerator.Native;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativePersistentVolumeCommandHandler
{
  readonly PersistentVolumeGenerator _generator = new();

  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1PersistentVolume()
    {
      ApiVersion = "v1",
      Kind = "PersistentVolume",
      Metadata = new V1ObjectMeta()
      {
        Name = "my-persistent-volume"
      },
      Spec = new V1PersistentVolumeSpec()
      {
        Capacity = new Dictionary<string, ResourceQuantity>()
        {
          ["storage"] = new ResourceQuantity("5Gi")
        },
        AccessModes = ["ReadWriteOnce"],
        StorageClassName = "my-storage-class",
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
