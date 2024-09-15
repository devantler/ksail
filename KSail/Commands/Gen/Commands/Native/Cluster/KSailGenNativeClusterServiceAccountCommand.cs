
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterServiceAccountCommand : Command
{
  public KSailGenNativeClusterServiceAccountCommand() : base("service-account", "Generate a 'core/v1/ServiceAccount' resource.")
  {
  }
}
