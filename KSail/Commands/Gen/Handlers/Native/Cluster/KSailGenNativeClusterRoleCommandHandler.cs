using Devantler.KubernetesGenerator.Native.Cluster;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Cluster;

class KSailGenNativeClusterRoleCommandHandler
{
  readonly RoleGenerator _generator = new();

  internal async Task HandleAsync(string outputPath, CancellationToken cancellationToken = default)
  {
    var model = new V1Role()
    {
      ApiVersion = "rbac.authorization.k8s.io/v1",
      Kind = "Role",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
        NamespaceProperty = "<namespace>",
      },
      Rules = [
        new V1PolicyRule()
        {
          ApiGroups = ["<api-group>"],
          Resources = ["<resource>"],
          Verbs = ["<verb>"],
        }
      ]
    };
    await _generator.GenerateAsync(model, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
