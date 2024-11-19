using Devantler.KubernetesGenerator.Native.Cluster;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Cluster;

class KSailGenNativeClusterStorageVersionMigrationCommandHandler
{
  readonly StorageVersionMigrationGenerator _generator = new();

  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1alpha1StorageVersionMigration()
    {
      ApiVersion = "storagemigration.k8s.io/v1alpha1",
      Kind = "StorageVersionMigration",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      Spec = new V1alpha1StorageVersionMigrationSpec()
      {
        Resource = new V1alpha1GroupVersionResource()
        {
          Group = "<group>",
          Version = "<version>",
          Resource = "<resource>",
        }
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
