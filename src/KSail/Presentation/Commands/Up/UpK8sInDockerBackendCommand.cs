using System.CommandLine;
using System.Globalization;
using KSail.Enums;
using KSail.Provisioners.Cluster;
using KSail.Utils;

namespace KSail.Presentation.Commands.Up;

/// <summary>
/// The 'up k8s-in-docker-backend' command responsible for creating K8s clusters.
/// </summary>
public class UpK8sInDockerBackendCommand : Command
{
  readonly IClusterProvisioner _provisioner;
  /// <summary>
  /// Initializes a new instance of the <see cref="UpK8sInDockerBackendCommand"/> class.
  /// </summary>
  /// <param name="k8sInDockerBackend">An enum value representing the K8s-in-Docker backend.</param>
  /// <param name="nameOption">The -n, --name option.</param>
  /// <param name="manifestsPathOption">The -mp, --manifests-path option.</param>
  /// <param name="fluxKustomizationPathOption">The -fkp, --flux-kustomization-path option.</param>
  public UpK8sInDockerBackendCommand(
    K8sInDockerBackend k8sInDockerBackend,
    Option<string> nameOption,
    Option<string> manifestsPathOption,
    Option<string> fluxKustomizationPathOption
  ) : base(k8sInDockerBackend.ToString().ToLower(CultureInfo.InvariantCulture), "create a K8s cluster ")
  {
    _provisioner = IClusterProvisioner.GetProvisioner(k8sInDockerBackend);

    var configPathOption = new Option<string>(["-c", "--config"], "path to the cluster configuration file");
    AddOptions(nameOption, manifestsPathOption, fluxKustomizationPathOption, configPathOption);

    this.SetHandler(async (name, manifestsPath, fluxKustomizationPath, configPath) =>
    {
      if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(configPath))
      {
        string shouldUseConfig =
          k8sInDockerBackend == K8sInDockerBackend.Talos || configPathOption is null || !string.IsNullOrEmpty(name) ?
            "n" : ConsoleUtils.Prompt("Would you like to use a cluster configuration file? (y/n)", filter: RegexFilters.YesNoFilter());
        if (shouldUseConfig == "y")
        {
          configPath = ConsoleUtils.Prompt("Please enter the path to the cluster configuration file", filter: RegexFilters.PathFilter());
        }
        else
        {
          name = ConsoleUtils.Prompt("Please enter the name of the cluster to create");
          manifestsPath = ConsoleUtils.Prompt("Please enter the path to the manifests directory (default: ./k8s)", "./k8s", RegexFilters.PathFilter());
          fluxKustomizationPath = ConsoleUtils.Prompt($"Please enter the path to the Flux Kustomization files relative to the 'manifestsPath' (default: ./clusters/{name}/flux)", "./clusters/{name}/flux", RegexFilters.PathFilter());
        }
      }

      // Check if a cluster with the specified name already exists.

      // Print that we are destroying the cluster prior to recreating it.
      Console.WriteLine($"🔥 Destroying '{name}' cluster prior to creation...");
      await _provisioner.DeprovisionAsync(name);
      Console.WriteLine();

      if (!string.IsNullOrEmpty(configPath))
      {
        Console.WriteLine($"🚀 Creating cluster from '{configPath}' with manifest path '{manifestsPath}' and flux kustomization path '{fluxKustomizationPath}'...");
      }
      else
      {
        Console.WriteLine($"🚀 Creating '{name}' cluster with manifest path '{manifestsPath}' and flux kustomization path '{fluxKustomizationPath}'...");
      }
      await _provisioner.ProvisionAsync(name, manifestsPath, fluxKustomizationPath);
      Console.WriteLine();
    }, nameOption, manifestsPathOption, fluxKustomizationPathOption, configPathOption);
  }

  void AddOptions(Option<string> nameOption, Option<string> manifestsPathOption, Option<string> fluxKustomizationPathOption, Option<string> configPathOption)
  {
    AddOption(nameOption);
    AddOption(manifestsPathOption);
    AddOption(fluxKustomizationPathOption);
    if (configPathOption is not null)
    {
      AddOption(configPathOption);
    }
  }
}
