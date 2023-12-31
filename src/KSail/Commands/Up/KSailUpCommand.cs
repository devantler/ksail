using System.CommandLine;

namespace KSail.Commands.Up;

sealed class KSailUpCommand : Command
{
  internal KSailUpCommand() : base("up", "create a K8s cluster")
  {
    AddCommand(new KSailUpK3dCommand());
    this.SetHandler(() => _ = this.InvokeAsync("--help"));
  }
}
