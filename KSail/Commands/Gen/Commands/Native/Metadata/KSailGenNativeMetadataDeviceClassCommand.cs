
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataDeviceClassCommand : Command
{
  public KSailGenNativeMetadataDeviceClassCommand() : base("device-class", "Generate a 'resource.k8s.io/v1alpha3/DeviceClass' resource.")
  {
  }
}
