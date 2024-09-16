
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Cluster;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterRoleCommand : Command
{
  readonly FileOutputOption _outputOption = new("./role.yaml");
  readonly KSailGenNativeClusterRoleCommandHandler _handler = new();
  public KSailGenNativeClusterRoleCommand() : base("role", "Generate a 'rbac.authorization.k8s.io/v1/Role' resource.")
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
