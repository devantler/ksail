using System.CommandLine;
using KSail.Options;

namespace KSail.Commands.Down;

sealed class KSailDownCommand : Command
{
  readonly NameOption nameOption = new("name of the cluster to destroy") { IsRequired = true };
  internal KSailDownCommand() : base("down", "destroy a K8s cluster")
  {
    AddCommand(new KSailDownK3dCommand(nameOption));
    this.SetHandler(() => _ = this.InvokeAsync("--help"));
  }
}
