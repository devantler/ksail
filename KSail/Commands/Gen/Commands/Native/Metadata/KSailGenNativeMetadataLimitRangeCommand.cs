
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataLimitRangeCommand : Command
{
  public KSailGenNativeMetadataLimitRangeCommand() : base("limit-range", "Generate a 'core/v1/LimitRange' resource.")
  {
  }
}
