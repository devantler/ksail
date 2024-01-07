using System.CommandLine;

namespace KSail.Commands.Up;

sealed class UpCommand : Command
{
  internal UpCommand() : base("up", "create a K8s cluster")
  {
    AddCommand(new KSailUpK3dCommand());
    this.SetHandler(() => _ = this.InvokeAsync("--help"));
  }
}
