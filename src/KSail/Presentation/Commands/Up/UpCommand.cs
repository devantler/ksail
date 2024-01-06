using System.CommandLine;
using KSail.Enums;
using KSail.Presentation.Commands.Up.Options;
using KSail.Presentation.Options;

namespace KSail.Presentation.Commands.Up;

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
    AddCommands();
    this.SetHandler(() => _ = this.InvokeAsync("--help"));
  }

  void AddCommands()
  {
    var nameOption = new NameOption("name of the cluster");
    var manifestsPathOption = new ManifestsPathOption();
    var fluxKustomizationPathOption = new FluxKustomizationPathOption(nameOption.Name);

    foreach (var k8sInDockerBackend in Enum.GetValues<K8sInDockerBackend>())
    {
      AddCommand(new UpK8sInDockerBackendCommand(k8sInDockerBackend, nameOption, manifestsPathOption, fluxKustomizationPathOption));
    }
  }
}
