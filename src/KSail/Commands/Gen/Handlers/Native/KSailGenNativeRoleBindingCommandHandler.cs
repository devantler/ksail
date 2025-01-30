using Devantler.KubernetesGenerator.Native;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativeRoleBindingCommandHandler
{
  readonly RoleBindingGenerator _generator = new();

  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1RoleBinding()
    {
      ApiVersion = "rbac.authorization.k8s.io/v1",
      Kind = "RoleBinding",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
        NamespaceProperty = "<namespace>",
      },
      Subjects =
      [
        new Rbacv1Subject()
        {
          Kind = "User",
          Name = "<user-name>",
          ApiGroup = "rbac.authorization.k8s.io",
        }
      ],
      RoleRef = new V1RoleRef()
      {
        Kind = "Role",
        Name = "<cluster-role-name>",
        ApiGroup = "rbac.authorization.k8s.io",
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
