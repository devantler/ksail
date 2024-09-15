
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterRoleBindingCommand : Command
{
  public KSailGenNativeClusterRoleBindingCommand() : base("role-binding", "Generate a 'rbac.authorization.k8s.io/v1/RoleBinding' resource.")
  {
  }
}
