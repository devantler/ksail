using System.CommandLine;
using KSail.Options;

namespace KSail.Commands.Down;

sealed class DownCommand : Command
{
  internal DownCommand() : base("down", "destroy a K8s cluster")
  {
    var nameOption = new NameOption("name of the cluster to destroy");
    AddCommand(new KSailDownK3dCommand(nameOption));
    this.SetHandler(() => _ = this.InvokeAsync("--help"));
  }
}
