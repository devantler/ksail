using Devantler.KubernetesGenerator.Native;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativeClusterRoleCommandHandler
{
  readonly ClusterRoleGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
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
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
