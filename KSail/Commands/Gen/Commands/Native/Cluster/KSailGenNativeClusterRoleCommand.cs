
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterRoleCommand : Command
{
  public KSailGenNativeClusterRoleCommand() : base("role", "Generate a 'rbac.authorization.k8s.io/v1/Role' resource.")
  {
  }
}
