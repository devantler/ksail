using System.ComponentModel;
using System.Runtime.InteropServices;
using KSail.Models.CLI;
using KSail.Models.CNI;
using KSail.Models.Connection;
using KSail.Models.DeploymentTool;
using KSail.Models.MirrorRegistry;
using KSail.Models.Project;
using KSail.Models.SecretManager;
using KSail.Models.Template;
using YamlDotNet.Serialization;

namespace KSail.Models;

/// <summary>
/// The KSail cluster specification.
/// </summary>
public class KSailClusterSpec
{
  /// <summary>
  /// The options for connecting to the KSail cluster.
  /// </summary>
  [Description("The options for connecting to the KSail cluster.")]
  public KSailConnectionOptions Connection { get; set; } = new();

  /// <summary>
  /// The options for the KSail project.
  /// </summary>
  [Description("The options for the KSail project.")]
  public KSailProjectOptions Project { get; set; } = new();

  /// <summary>
  /// The options for the Flux deployment tool.
  /// </summary>
  [Description("The options for the Flux deployment tool.")]
  [YamlIgnore]
  public KSailFluxDeploymentToolOptions FluxDeploymentToolOptions { get; set; } = new();

  /// <summary>
  /// The options for the Kustomize template.
  /// </summary>
  [Description("The options for the Kustomize template.")]
  [YamlIgnore]
  public KSailKustomizeTemplateOptions KustomizeTemplateOptions { get; set; } = new();

  /// <summary>
  /// The options for the SOPS Secret Manager.
  /// </summary>
  [Description("The options for the SOPS Secret Manager.")]
  [YamlMember(Alias = "sopsSecretManagerOptions")]
  [YamlIgnore]
  public KSailSOPSSecretManagerOptions SOPSSecretManagerOptions { get; set; } = new();

  /// <summary>
  /// The options for the Cilium CNI.
  /// </summary>
  [Description("The options for the Cilium CNI.")]
  [YamlIgnore]
  public KSailCiliumCNIOptions CiliumCNIOptions { get; set; } = new();

  /// <summary>
  /// The options for mirror registries.
  /// </summary>
  [Description("The options for mirror registries.")]
  [YamlIgnore]
  public KSailMirrorRegistryOptions MirrorRegistryOptions { get; set; } = new();

  /// <summary>
  /// The CLI options.
  /// </summary>
  [Description("The CLI options.")]
  [YamlMember(Alias = "cliOptions")]
  [YamlIgnore]
  public KSailCLIOptions CLIOptions { get; set; } = new();

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailClusterSpec"/> class.
  /// </summary>
  public KSailClusterSpec() => SetOCISourceUriBasedOnOS();

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailClusterSpec"/> class.
  /// </summary>
  /// <param name="name"></param>
  public KSailClusterSpec(string name)
  {
    SetOCISourceUriBasedOnOS();
    Connection = new KSailConnectionOptions
    {
      Context = $"kind-{name}"
    };
    KustomizeTemplateOptions = new KSailKustomizeTemplateOptions
    {
      RootKustomizationDirectory = $"k8s/clusters/{name}/flux-system"
    };
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailClusterSpec"/> class with the specified distribution.
  /// </summary>
  /// <param name="name"></param>
  /// <param name="distribution"></param>
  public KSailClusterSpec(string name, KSailKubernetesDistribution distribution) : this(name)
  {
    SetOCISourceUriBasedOnOS();
    Connection = new KSailConnectionOptions
    {
      Context = distribution switch
      {
        KSailKubernetesDistribution.Native => $"kind-{name}",
        KSailKubernetesDistribution.K3s => $"k3d-{name}",
        _ => $"kind-{name}"
      }
    };
    Project = new KSailProjectOptions
    {
      Distribution = distribution,
      DistributionConfigPath = distribution switch
      {
        KSailKubernetesDistribution.Native => "kind-config.yaml",
        KSailKubernetesDistribution.K3s => "k3d-config.yaml",
        _ => "kind-config.yaml"
      }
    };
    KustomizeTemplateOptions = new KSailKustomizeTemplateOptions
    {
      RootKustomizationDirectory = $"k8s/clusters/{name}/flux-system"
    };
  }

  void SetOCISourceUriBasedOnOS()
  {
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
      FluxDeploymentToolOptions = new KSailFluxDeploymentToolOptions(new Uri("oci://172.17.0.1:5555/ksail-registry"));
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
      FluxDeploymentToolOptions = new KSailFluxDeploymentToolOptions(new Uri("oci://host.docker.internal:5555/ksail-registry"));
    }
  }
}
