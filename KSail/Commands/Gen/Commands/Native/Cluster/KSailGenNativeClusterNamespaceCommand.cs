
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterNamespaceCommand : Command
{
  public KSailGenNativeClusterNamespaceCommand() : base("namespace", "Generate a 'core/v1/Namespace' resource.")
  {
  }
}
