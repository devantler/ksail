
using Devantler.KubernetesGenerator.Native.ConfigAndStorage;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativeStorageClassCommandHandler
{
  readonly StorageClassGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1StorageClass
    {
      ApiVersion = "storage.k8s.io/v1",
      Kind = "StorageClass",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      Provisioner = "<provisioner>",
      Parameters = new Dictionary<string, string>()
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
