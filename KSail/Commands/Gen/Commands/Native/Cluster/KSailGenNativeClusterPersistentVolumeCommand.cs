
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterPersistentVolumeCommand : Command
{
  public KSailGenNativeClusterPersistentVolumeCommand() : base("persistent-volume", "Generate a 'core/v1/PersistentVolume' resource.")
  {
  }
}
