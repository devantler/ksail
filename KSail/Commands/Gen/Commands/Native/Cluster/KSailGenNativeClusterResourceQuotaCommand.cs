
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterResourceQuotaCommand : Command
{
  public KSailGenNativeClusterResourceQuotaCommand() : base("resource-quota", "Generate a 'core/v1/ResourceQuota' resource.")
  {
  }
}
