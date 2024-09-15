
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterRuntimeClassCommand : Command
{
  public KSailGenNativeClusterRuntimeClassCommand() : base("runtime-class", "Generate a 'node.k8s.io/v1/RuntimeClass' resource.")
  {
  }
}
