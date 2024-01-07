using System.CommandLine;
using KSail.Options;

namespace KSail.Commands.Down;

sealed class KSailDownCommand : Command
{
  readonly NameOption _nameOption = new("name of the cluster to destroy");
  internal KSailDownCommand() : base("down", "destroy a K8s cluster")
  {
    AddCommand(new KSailDownK3dCommand(_nameOption));
    this.SetHandler(() => _ = this.InvokeAsync("--help"));
  }
}
