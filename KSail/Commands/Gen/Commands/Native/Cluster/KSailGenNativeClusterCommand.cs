
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Cluster;

class KSailGenNativeClusterCommand : Command
{
  public KSailGenNativeClusterCommand(IConsole? console = default) : base("cluster", "Generate a native Kubernetes resource from the cluster category.")
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
    AddCommand(new KSailGenNativeClusterAPIServiceCommand());
    AddCommand(new KSailGenNativeClusterClusterRoleBindingCommand());
    AddCommand(new KSailGenNativeClusterClusterRoleCommand());
    AddCommand(new KSailGenNativeClusterFlowSchemaCommand());
    AddCommand(new KSailGenNativeClusterIPAddressCommand());
    AddCommand(new KSailGenNativeClusterNamespaceCommand());
    AddCommand(new KSailGenNativeClusterNetworkPolicyCommand());
    AddCommand(new KSailGenNativeClusterPersistentVolumeCommand());
    AddCommand(new KSailGenNativeClusterPriorityLevelConfigurationCommand());
    AddCommand(new KSailGenNativeClusterResourceQuotaCommand());
    AddCommand(new KSailGenNativeClusterRoleBindingCommand());
    AddCommand(new KSailGenNativeClusterRoleCommand());
    AddCommand(new KSailGenNativeClusterRuntimeClassCommand());
    AddCommand(new KSailGenNativeClusterServiceAccountCommand());
    AddCommand(new KSailGenNativeClusterServiceCIDRCommand());
    AddCommand(new KSailGenNativeClusterStorageVersionMigrationCommand());
  }
}
