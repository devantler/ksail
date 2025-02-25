using System.ComponentModel;
using KSail.Models.Connection;
using KSail.Models.DeploymentTool;
using KSail.Models.Distribution;
using KSail.Models.IngressController;
using KSail.Models.LocalRegistry;
using KSail.Models.MirrorRegistry;
using KSail.Models.Project;
using KSail.Models.Project.Enums;
using KSail.Models.SecretManager;
using KSail.Models.Template;
using KSail.Models.Validation;
using KSail.Models.WaypointController;
using YamlDotNet.Serialization;

namespace KSail.Models;

/// <summary>
/// The KSail cluster specification.
/// </summary>
public class KSailClusterSpec
{
  /// <summary>
  /// Options for connecting to the KSail cluster.
  /// </summary>
  [Description("The options for connecting to the KSail cluster.")]
  public KSailConnection Connection { get; set; } = new();

  /// <summary>
  /// Options for the KSail project.
  /// </summary>
  [Description("The options for the KSail project.")]
  public KSailProject Project { get; set; } = new();

  /// <summary>
  /// Options for the Flux deployment tool.
  /// </summary>
  [Description("The options for the deployment tool.")]
  public KSailDeploymentTool DeploymentTool { get; set; } = new();

  /// <summary>
  /// Options for the distribution.
  /// </summary>
  public KSailDistribution Distribution { get; set; } = new();

  /// <summary>
  /// Options for the template.
  /// </summary>
  [Description("The options for the template.")]
  public KSailTemplate Template { get; set; } = new();

  /// <summary>
  /// Options for the Secret Manager.
  /// </summary>
  [Description("The options for the Secret Manager.")]
  public KSailSecretManager SecretManager { get; set; } = new();

  /// <summary>
  /// Options for the CNI.
  /// </summary>
  [Description("The options for the CNI.")]
  [YamlMember(Alias = "cni")]
  public KSailCNIType CNI { get; set; } = new();

  /// <summary>
  /// Options for the Ingress Controller.
  /// </summary>
  [Description("The options for the Ingress Controller.")]
  public KSailIngressController IngressController { get; set; } = new();

  /// <summary>
  /// Options for the Waypoint Controller.
  /// </summary>
  [Description("The options for the Waypoint Controller.")]
  public KSailWaypointController WaypointController { get; set; } = new();

  /// <summary>
  /// The local registry for storing deployment artifacts.
  /// </summary>
  [Description("The local registry for storing deployment artifacts.")]
  public KSailLocalRegistry LocalRegistry { get; set; } = new KSailLocalRegistry
  {
    Name = "ksail-registry",
    HostPort = 5555
  };

  /// <summary>
  /// The mirror registries to create for the KSail cluster.
  /// </summary>
  [Description("The mirror registries to create for the KSail cluster.")]
  public IEnumerable<KSailMirrorRegistry> MirrorRegistries { get; set; } = [
    new KSailMirrorRegistry { Name = "registry.k8s.io-proxy", HostPort = 5556, Proxy = new KSailMirrorRegistryProxy { Url = new Uri("https://registry.k8s.io") } },
    new KSailMirrorRegistry { Name = "docker.io-proxy", HostPort = 5557,  Proxy = new KSailMirrorRegistryProxy { Url = new Uri("https://registry-1.docker.io") } },
    new KSailMirrorRegistry { Name = "ghcr.io-proxy", HostPort = 5558, Proxy = new KSailMirrorRegistryProxy { Url = new Uri("https://ghcr.io") } },
    new KSailMirrorRegistry { Name = "gcr.io-proxy", HostPort = 5559, Proxy = new KSailMirrorRegistryProxy { Url = new Uri("https://gcr.io") } },
    new KSailMirrorRegistry { Name = "mcr.microsoft.com-proxy", HostPort = 5560, Proxy = new KSailMirrorRegistryProxy { Url = new Uri("https://mcr.microsoft.com") } },
    new KSailMirrorRegistry { Name = "quay.io-proxy", HostPort = 5561, Proxy = new KSailMirrorRegistryProxy { Url = new Uri("https://quay.io") } },
  ];

  /// <summary>
  /// Options for validating the KSail cluster.
  /// </summary>
  [Description("Options for validating the KSail cluster.")]
  public KSailValidation Validation { get; set; } = new();

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailClusterSpec"/> class.
  /// </summary>
  public KSailClusterSpec() => SetOCISourceUri();

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailClusterSpec"/> class.
  /// </summary>
  /// <param name="name"></param>
  public KSailClusterSpec(string name)
  {
    SetOCISourceUri();
    Connection = new KSailConnection
    {
      Context = $"kind-{name}"
    };
    Template.Kustomize = new KSailTemplateKustomize
    {
      Root = $"k8s/clusters/{name}/flux-system"
    };
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailClusterSpec"/> class with the specified distribution.
  /// </summary>
  /// <param name="name"></param>
  /// <param name="distribution"></param>
  public KSailClusterSpec(string name, KSailKubernetesDistributionType distribution) : this(name)
  {
    SetOCISourceUri(distribution);
    Connection = new KSailConnection
    {
      Context = distribution switch
      {
        KSailKubernetesDistributionType.Native => $"kind-{name}",
        KSailKubernetesDistributionType.K3s => $"k3d-{name}",
        _ => $"kind-{name}"
      }
    };
    Project = new KSailProject
    {
      Distribution = distribution,
      DistributionConfigPath = distribution switch
      {
        KSailKubernetesDistributionType.Native => "kind-config.yaml",
        KSailKubernetesDistributionType.K3s => "k3d-config.yaml",
        _ => "kind-config.yaml"
      }
    };
    Template.Kustomize = new KSailTemplateKustomize
    {
      Root = $"k8s/clusters/{name}/flux-system"
    };
  }

  void SetOCISourceUri(KSailKubernetesDistributionType distribution = KSailKubernetesDistributionType.Native)
  {
    DeploymentTool.Flux = distribution switch
    {
      KSailKubernetesDistributionType.Native => new KSailFluxDeploymentTool(new Uri("oci://ksail-registry:5000/ksail-registry")),
      KSailKubernetesDistributionType.K3s => new KSailFluxDeploymentTool(new Uri("oci://host.k3d.internal:5555/ksail-registry")),
      _ => new KSailFluxDeploymentTool(new Uri("oci://ksail-registry:5000/ksail-registry")),
    };
  }
}
