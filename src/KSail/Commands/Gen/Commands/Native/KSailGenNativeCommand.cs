
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native;

class KSailGenNativeCommand : Command
{

  public KSailGenNativeCommand(IConsole? console = default) : base("native", "Generate a native Kubernetes resource.")
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
    AddCommand(new KSailGenNativeAPIServiceCommand());
    AddCommand(new KSailGenNativeClusterRoleBindingCommand());
    AddCommand(new KSailGenNativeClusterRoleCommand());
    AddCommand(new KSailGenNativeFlowSchemaCommand());
    AddCommand(new KSailGenNativeNamespaceCommand());
    AddCommand(new KSailGenNativeNetworkPolicyCommand());
    AddCommand(new KSailGenNativePersistentVolumeCommand());
    AddCommand(new KSailGenNativePriorityLevelConfigurationCommand());
    AddCommand(new KSailGenNativeResourceQuotaCommand());
    AddCommand(new KSailGenNativeRoleBindingCommand());
    AddCommand(new KSailGenNativeRoleCommand());
    AddCommand(new KSailGenNativeRuntimeClassCommand());
    AddCommand(new KSailGenNativeAccountCommand());
    AddCommand(new KSailGenNativeStorageVersionMigrationCommand());

    AddCommand(new KSailGenNativeConfigMapCommand());
    AddCommand(new KSailGenNativeCSIDriverCommand());
    AddCommand(new KSailGenNativePersistentVolumeClaimCommand());
    AddCommand(new KSailGenNativeSecretCommand());
    AddCommand(new KSailGenNativeStorageClassCommand());
    AddCommand(new KSailGenNativeVolumeAttributesClassCommand());

    AddCommand(new KSailGenNativeClusterTrustBundleCommand());
    AddCommand(new KSailGenNativeCustomResourceDefinitionCommand());
    AddCommand(new KSailGenNativeHorizontalPodAutoscalerCommand());
    AddCommand(new KSailGenNativeLimitRangeCommand());
    AddCommand(new KSailGenNativeMutatingWebhookConfigurationCommand());
    AddCommand(new KSailGenNativePodDisruptionBudgetCommand());
    AddCommand(new KSailGenNativePriorityClassCommand());
    AddCommand(new KSailGenNativeValidatingAdmissionPolicyBindingCommand());
    AddCommand(new KSailGenNativeValidatingAdmissionPolicyCommand());
    AddCommand(new KSailGenNativeValidatingWebhookConfigurationCommand());

    AddCommand(new KSailGenNativeIngressClassCommand());
    AddCommand(new KSailGenNativeIngressCommand());
    AddCommand(new KSailGenNativeServiceCommand());

    AddCommand(new KSailGenNativeWorkloadsCronJobCommand());
    AddCommand(new KSailGenNativeWorkloadsDaemonSetCommand());
    AddCommand(new KSailGenNativeWorkloadsDeploymentCommand());
    AddCommand(new KSailGenNativeWorkloadsJobCommand());
    AddCommand(new KSailGenNativeWorkloadsReplicaSetCommand());
    AddCommand(new KSailGenNativeWorkloadsStatefulSetCommand());
  }
}
