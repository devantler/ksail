using System.CommandLine;
using System.Globalization;
using KSail.Enums;
using KSail.Provisioners.Cluster;
using KSail.Utils;

namespace KSail.Presentation.Commands.Down;

/// <summary>
/// The 'down k8s-in-docker-backend' command responsible for destroying K8s clusters.
/// </summary>
public class DownK8sInDockerBackendCommand : Command
{
  readonly IClusterProvisioner _provisioner;

  /// <summary>
  /// Initializes a new instance of the <see cref="DownK8sInDockerBackendCommand"/> class.
  /// </summary>
  /// <param name="k8sInDockerBackend">An enum value representing the K8s-in-Docker backend.</param>
  /// <param name="nameOption">The -n, --name option.</param>
  public DownK8sInDockerBackendCommand(
    K8sInDockerBackend k8sInDockerBackend,
    Option<string> nameOption
  ) : base(k8sInDockerBackend.ToString().ToLower(CultureInfo.InvariantCulture), "destroy a K8s cluster ")
  {
    _provisioner = IClusterProvisioner.GetProvisioner(k8sInDockerBackend);
    AddOption(nameOption);

    this.SetHandler(async (name) =>
    {
      name ??= ConsoleUtils.Prompt("Please enter the name of the cluster to destroy");

      Console.WriteLine($"🔥 Destroying '{name}' cluster...");
      await _provisioner.DeprovisionAsync(name);
    }, nameOption);
  }
}
