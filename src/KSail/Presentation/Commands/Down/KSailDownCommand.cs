using System.CommandLine;
using KSail.Presentation.Options;

namespace KSail.Presentation.Commands.Down;

/// <summary>
/// The 'down' command responsible for destroying K8s clusters.
/// </summary>
public class DownCommand : Command
{
  /// <summary>
  /// Initializes a new instance of the <see cref="DownCommand"/> class.
  /// </summary>
  public DownCommand() : base("down", "destroy a K8s cluster")
  {
    var nameOption = new NameOption("name of the cluster to destroy");
    AddCommand(new KSailDownK3dCommand(nameOption));
    this.SetHandler(() => _ = this.InvokeAsync("--help"));
  }
}
