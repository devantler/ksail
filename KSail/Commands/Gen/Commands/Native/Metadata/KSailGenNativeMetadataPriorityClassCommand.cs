
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataPriorityClassCommand : Command
{
  public KSailGenNativeMetadataPriorityClassCommand() : base("priority-class", "Generate a 'scheduling.k8s.io/v1/PriorityClass' resource.")
  {
  }
}
