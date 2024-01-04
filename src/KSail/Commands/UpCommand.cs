using System.CommandLine;

namespace KSail.Commands;

/// <summary>
/// The 'up' command responsible for creating new K8s cluster.
/// </summary>
public class UpCommand : Command
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UpCommand"/> class.
  /// </summary>
  public UpCommand() : base("up", "create a new K8s cluster")
  {
    AddCommand(new Command("k3d", "create a new K3d cluster"));
    AddCommand(new Command("talos", "create a new Talos cluster"));
    this.SetHandler(() => _ = this.InvokeAsync("--help"));
  }
}
