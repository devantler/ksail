
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataPodDisruptionBudgetCommand : Command
{
  public KSailGenNativeMetadataPodDisruptionBudgetCommand() : base("pod-disruption-budget", "Generate a 'policy/v1/PodDisruptionBudget' resource.")
  {
  }
}
