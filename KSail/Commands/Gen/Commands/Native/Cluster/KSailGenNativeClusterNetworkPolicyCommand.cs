
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterNetworkPolicyCommand : Command
{
  public KSailGenNativeClusterNetworkPolicyCommand() : base("network-policy", "Generate a 'networking.k8s.io/v1/NetworkPolicy' resource.")
  {
  }
}
