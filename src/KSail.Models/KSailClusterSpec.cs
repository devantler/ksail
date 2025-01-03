using System.ComponentModel;
using KSail.Models.CLI;
using KSail.Models.Project;
using KSail.Models.Registry;
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
  /// The registries to create for the KSail cluster.
  /// </summary>
  [Description("The registries to create for the KSail cluster")]
  public IEnumerable<KSailRegistry> Registries { get; set; } = [
    new KSailRegistry { Name = "ksail-registry", HostPort = 5555, IsGitOpsSource = true },
    new KSailRegistry { Name = "registry.k8s.io", HostPort = 5556, Proxy = new KSailRegistryProxy { Url = new Uri("https://registry.k8s.io") } },
    new KSailRegistry { Name = "docker.io", HostPort = 5557,  Proxy = new KSailRegistryProxy { Url = new Uri("https://registry-1.docker.io") } },
    new KSailRegistry { Name = "ghcr.io", HostPort = 5558, Proxy = new KSailRegistryProxy { Url = new Uri("https://ghcr.io") } },
    new KSailRegistry { Name = "gcr.io", HostPort = 5559, Proxy = new KSailRegistryProxy { Url = new Uri("https://gcr.io") } },
    new KSailRegistry { Name = "mcr.microsoft.com", HostPort = 5560, Proxy = new KSailRegistryProxy { Url = new Uri("https://mcr.microsoft.com") } },
    new KSailRegistry { Name = "quay.io", HostPort = 5561, Proxy = new KSailRegistryProxy { Url = new Uri("https://quay.io") } },
  ];

  /// <summary>
  /// The CLI options.
  /// </summary>
  [YamlMember(Alias = "cli")]
  [Description("The CLI options.")]
  public KSailCLIOptions CLI { get; set; } = new();

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailClusterSpec"/> class.
  /// </summary>
  public KSailClusterSpec()
  {
    Connection = new KSailConnectionOptions();
    Project = new KSailProjectOptions();
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailClusterSpec"/> class.
  /// </summary>
  /// <param name="name"></param>
  public KSailClusterSpec(string name)
  {
    Connection = new KSailConnectionOptions
    {
      Context = $"kind-{name}"
    };
    Project = new KSailProjectOptions
    {
      KustomizationDirectory = $"./k8s/clusters/{name}/flux-system",
    };
    Project.KustomizationDirectory = $"{Project.ManifestsDirectory}/clusters/{name}/flux-system";
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailClusterSpec"/> class with the specified distribution.
  /// </summary>
  /// <param name="name"></param>
  /// <param name="distribution"></param>
  public KSailClusterSpec(string name, KSailKubernetesDistribution distribution) : this(name)
  {
    Project.Distribution = distribution;
    Project.ConfigPath = $"./{distribution.ToString().ToLower()}-config.yaml";
    Connection.Context = $"{distribution.ToString().ToLower()}-{name}";
  }
}
