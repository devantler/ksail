
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterIPAddressCommand : Command
{
  public KSailGenNativeClusterIPAddressCommand() : base("ip-address", "Generate a 'networking.k8s.io/v1beta1/IPAddress' resource.")
  {
  }
}
