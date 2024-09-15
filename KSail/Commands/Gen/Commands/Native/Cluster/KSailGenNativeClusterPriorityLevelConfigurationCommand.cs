
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterPriorityLevelConfigurationCommand : Command
{
  public KSailGenNativeClusterPriorityLevelConfigurationCommand() : base("priority-level-configuration", "Generate a 'flowcontrol.apiserver.k8s.io/v1/PriorityLevelConfiguration' resource.")
  {
  }
}
