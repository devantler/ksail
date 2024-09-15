
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataHorizontalPodAutoscalerCommand : Command
{
  public KSailGenNativeMetadataHorizontalPodAutoscalerCommand() : base("horizontal-pod-autoscaler", "Generate a 'autoscaling/v2/HorizontalPodAutoscaler' resource.")
  {
  }
}
