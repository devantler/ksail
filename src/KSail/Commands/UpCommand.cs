using System.CommandLine;

namespace KSail.Commands;

public class UpCommand : Command
{
  public UpCommand() : base("up", "create a new K8s cluster in Docker")
  {
    AddCommand(new Command("k3d", "create a new K3d cluster in Docker"));
    AddCommand(new Command("talos", "create a new Talos cluster in Docker"));
    this.SetHandler(() =>
    {
      _ = this.InvokeAsync("--help");
    });
  }
}
