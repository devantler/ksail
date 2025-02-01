using Devantler.KubernetesGenerator.Native;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativeClusterRoleBindingCommandHandler
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
        Name = "my-cluster-role-binding"
      },
      Subjects =
      [
        new Rbacv1Subject()
        {
          Kind = "Group",
          Name = "my-group",
          ApiGroup = "rbac.authorization.k8s.io"
        }
      ],
      RoleRef = new V1RoleRef()
      {
        Kind = "ClusterRole",
        Name = "my-cluster-role",
        ApiGroup = "rbac.authorization.k8s.io",
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
