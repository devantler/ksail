using Devantler.KubernetesGenerator.Native.Cluster;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Cluster;

class KSailGenNativeClusterClusterRoleCommandHandler
{
  readonly ClusterRoleGenerator _generator = new();
  internal async Task HandleAsync(string outputPath, CancellationToken cancellationToken)
  {
    var model = new V1ClusterRole()
    {
      ApiVersion = "rbac.authorization.k8s.io/v1",
      Kind = "ClusterRole",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      Rules =
      [
        new V1PolicyRule()
        {
          ApiGroups = [""],
          Resources = ["secrets"],
          Verbs = ["get", "watch", "list"]
        }
      ]
    };
    await _generator.GenerateAsync(model, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
