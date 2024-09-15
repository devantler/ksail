
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterClusterRoleCommand : Command
{
  readonly FileOutputOption _outputOption = new("./cluster-role.yaml");
  readonly KSailGenNativeClusterClusterRoleCommandHandler _handler = new();
  public KSailGenNativeClusterClusterRoleCommand() : base("cluster-role", "Generate a 'rbac.authorization.k8s.io/v1/ClusterRole' resource.")
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
