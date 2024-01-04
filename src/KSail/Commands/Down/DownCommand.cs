using System.CommandLine;
using KSail.Enums;
using KSail.Options;

namespace KSail.Commands.Down;

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
    foreach (var k8sInDockerBackend in Enum.GetValues<K8sInDockerBackend>())
    {
      AddCommand(new DownK8sInDockerBackendCommand(k8sInDockerBackend, nameOption));
    }

    this.SetHandler(() => _ = this.InvokeAsync("--help"));
  }
}
