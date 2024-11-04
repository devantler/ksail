
using Devantler.KubernetesGenerator.Native.Cluster;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Cluster;

class KSailGenNativeClusterClusterRoleBindingCommandHandler
{
  readonly ClusterRoleBindingGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1ClusterRoleBinding()
    {
      ApiVersion = "rbac.authorization.k8s.io/v1",
      Kind = "ClusterRoleBinding",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      Subjects =
      [
        new Rbacv1Subject()
        {
          Kind = "Group",
          Name = "<group>",
          ApiGroup = "rbac.authorization.k8s.io"
        }
      ],
      RoleRef = new V1RoleRef()
      {
        Kind = "ClusterRole",
        Name = "<cluster-role>",
        ApiGroup = "rbac.authorization.k8s.io",
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
