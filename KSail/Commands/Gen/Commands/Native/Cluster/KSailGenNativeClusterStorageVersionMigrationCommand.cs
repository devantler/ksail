
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterStorageVersionMigrationCommand : Command
{
  public KSailGenNativeClusterStorageVersionMigrationCommand() : base("storage-version-migration", "Generate a 'storagemigration.k8s.io/v1alpha1/StorageVersionMigration' resource.")
  {
  }
}
