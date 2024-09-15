
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterServiceCIDRCommand : Command
{
  public KSailGenNativeClusterServiceCIDRCommand() : base("service-cidr", "Generate a 'networking.k8s.io/v1beta1/ServiceCIDR' resource.")
  {
  }
}
