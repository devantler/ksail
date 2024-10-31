using KSail.Models.Commands.Check;
using KSail.Models.Commands.Debug;
using KSail.Models.Commands.Down;
using KSail.Models.Commands.Gen;
using KSail.Models.Commands.Init;
using KSail.Models.Commands.Lint;
using KSail.Models.Commands.List;
using KSail.Models.Commands.Sops;
using KSail.Models.Commands.Start;
using KSail.Models.Commands.Stop;
using KSail.Models.Commands.Up;
using KSail.Models.Commands.Update;
using KSail.Models.Registry;

namespace KSail.Models;

/// <summary>
/// The KSail cluster specification.
/// </summary>
public class KSailClusterSpec
{
  /// <summary>
  /// The path to the kubeconfig file.
  /// </summary>
  public string Kubeconfig { get; set; } = $"~/.kube/config";

  /// <summary>
  /// The kube context.
  /// </summary>
  public string Context { get; set; } = "default";

  /// <summary>
  /// The timeout for operations (in seconds).
  /// </summary>
  public int Timeout { get; set; } = 300;

  /// <summary>
  /// The path to the directory that contains the manifests.
  /// </summary>
  public string ManifestsDirectory { get; set; } = "./k8s";

  /// <summary>
  /// The relative path to the directory that contains the root kustomization file.
  /// </summary>
  public string KustomizationDirectory { get; set; }

  /// <summary>
  /// The path to the distribution configuration file.
  /// </summary>
  public string ConfigPath { get; set; }

  /// <summary>
  /// The Kubernetes distribution to use.
  /// </summary>
  public KSailKubernetesDistribution Distribution { get; set; } = KSailKubernetesDistribution.Kind;

  /// <summary>
  /// The GitOps tool to use.
  /// </summary>
  public KSailGitOpsTool GitOpsTool { get; set; } = KSailGitOpsTool.Flux;

  /// <summary>
  /// The container engine to use.
  /// </summary>
  public KSailContainerEngine ContainerEngine { get; set; } = KSailContainerEngine.Docker;

  /// <summary>
  /// Whether to enable SOPS support.
  /// </summary>
  public bool Sops { get; set; }

  /// <summary>
  /// The registries to create for the KSail cluster to reconcile flux artifacts, and to proxy and cache images.
  /// </summary>
  public IEnumerable<KSailRegistry> Registries { get; set; } = [
    new KSailRegistry { Name = "ksail-registry", HostPort = 5555, IsGitOpsOCISource = true },
    new KSailRegistry { Name = "registry.k8s.io", HostPort = 5556, Proxy = new KSailRegistryProxy { Url = new Uri("https://registry.k8s.io") } },
    new KSailRegistry { Name = "docker.io", HostPort = 5557,  Proxy = new KSailRegistryProxy { Url = new Uri("https://registry-1.docker.io") } },
    new KSailRegistry { Name = "ghcr.io", HostPort = 5558, Proxy = new KSailRegistryProxy { Url = new Uri("https://ghcr.io") } },
    new KSailRegistry { Name = "gcr.io", HostPort = 5559, Proxy = new KSailRegistryProxy { Url = new Uri("https://gcr.io") } },
    new KSailRegistry { Name = "mcr.microsoft.com", HostPort = 5560, Proxy = new KSailRegistryProxy { Url = new Uri("https://mcr.microsoft.com") } },
    new KSailRegistry { Name = "quay.io", HostPort = 5561, Proxy = new KSailRegistryProxy { Url = new Uri("https://quay.io") } },
  ];

  /// <summary>
  /// The options to use for the 'check' command.
  /// </summary>
  public KSailCheckOptions CheckOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'debug' command.
  /// </summary>
  public KSailDebugOptions DebugOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'down' command.
  /// </summary>
  public KSailDownOptions DownOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'gen' command.
  /// </summary>
  public KSailGenOptions GenOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'init' command.
  /// </summary>
  public KSailInitOptions InitOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'lint' command.
  /// </summary>
  public KSailLintOptions LintOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'list' command.
  /// </summary>
  public KSailListOptions ListOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'sops' command.
  /// </summary>
  public KSailSopsOptions SopsOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'start' command.
  /// </summary>
  public KSailStartOptions StartOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'stop' command.
  /// </summary>
  public KSailStopOptions StopOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'up' command.
  /// </summary>
  public KSailUpOptions UpOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'update' command.
  /// </summary>
  public KSailUpdateOptions UpdateOptions { get; set; } = new();

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailClusterSpec"/> class.
  /// </summary>
  /// <param name="name"></param>
  public KSailClusterSpec(string name)
  {
    KustomizationDirectory = $"{ManifestsDirectory}/clusters/{name}/flux-system";
    ConfigPath = $"{Distribution.ToString().ToLower(System.Globalization.CultureInfo.CurrentCulture)}-config.yaml";
  }
}
