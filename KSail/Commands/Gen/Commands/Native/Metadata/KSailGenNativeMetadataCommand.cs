
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataCommand : Command
{
  public KSailGenNativeMetadataCommand(IConsole? console = default) : base("metadata", "Generate a native Kubernetes resource from the metadata category.")
  {
    AddCommands();
    this.SetHandler(async (context) =>
      {
        context.ExitCode = await this.InvokeAsync("--help", console).ConfigureAwait(false);
      }
    );
  }

  void AddCommands()
  {
    AddCommand(new KSailGenNativeMetadataClusterTrustBundleCommand());
    AddCommand(new KSailGenNativeMetadataCustomResourceDefinitionCommand());
    AddCommand(new KSailGenNativeMetadataDeviceClassCommand());
    AddCommand(new KSailGenNativeMetadataHorizontalPodAutoscalerCommand());
    AddCommand(new KSailGenNativeMetadataLimitRangeCommand());
    AddCommand(new KSailGenNativeMetadataMutatingWebhookConfigurationCommand());
    AddCommand(new KSailGenNativeMetadataPodDisruptionBudgetCommand());
    AddCommand(new KSailGenNativeMetadataPriorityClassCommand());
    AddCommand(new KSailGenNativeMetadataValidatingAdmissionPolicyBindingCommand());
    AddCommand(new KSailGenNativeMetadataValidatingAdmissionPolicyCommand());
    AddCommand(new KSailGenNativeMetadataValidatingWebhookConfigurationCommand());
  }
}
