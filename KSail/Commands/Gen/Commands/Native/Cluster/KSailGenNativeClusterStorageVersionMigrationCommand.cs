
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterStorageVersionMigrationCommand : Command
{
  readonly FileOutputOption _outputOption = new("./storage-version-migration.yaml");
  readonly KSailGenNativeClusterStorageVersionMigrationCommandHandler _handler = new();
  public KSailGenNativeClusterStorageVersionMigrationCommand() : base("storage-version-migration", "Generate a 'storagemigration.k8s.io/v1alpha1/StorageVersionMigration' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        string outputPath = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        await _handler.HandleAsync(outputPath, context.GetCancellationToken()).ConfigureAwait(false);
      }
    );
  }
}
