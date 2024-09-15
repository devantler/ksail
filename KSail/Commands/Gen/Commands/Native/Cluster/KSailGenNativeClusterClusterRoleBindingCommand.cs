
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterClusterRoleBindingCommand : Command
{
  readonly FileOutputOption _outputOption = new("./cluster-role-binding.yaml");
  readonly KSailGenNativeClusterClusterRoleBindingCommandHandler _handler = new();
  public KSailGenNativeClusterClusterRoleBindingCommand() : base("cluster-role-binding", "Generate a 'rbac.authorization.k8s.io/v1/ClusterRoleBinding' resource.")
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
