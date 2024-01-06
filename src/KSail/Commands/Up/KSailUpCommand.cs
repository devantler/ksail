using System.CommandLine;

namespace KSail.Commands.Up;

/// <summary>
/// The 'up' command responsible for creating new K8s cluster.
/// </summary>
public class UpCommand : Command
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UpCommand"/> class.
  /// </summary>
  public UpCommand() : base("up", "create a K8s cluster")
  {
    AddCommand(new KSailUpK3dCommand());
    this.SetHandler(() => _ = this.InvokeAsync("--help"));
  }
}
