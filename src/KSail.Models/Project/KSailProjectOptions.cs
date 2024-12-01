namespace KSail.Models.Project;

/// <summary>
/// The options for the KSail project.
/// </summary>
public class KSailProjectOptions
{
  /// <summary>
  /// The path to the directory that contains the manifests.
  /// </summary>
  public string ManifestsDirectory { get; set; } = "./k8s";

  /// <summary>
  /// The relative path to the directory that contains the root kustomization file.
  /// </summary>
  public string KustomizationDirectory { get; set; } = "./k8s/clusters/ksail-default/flux-system";

  /// <summary>
  /// The path to the distribution configuration file.
  /// </summary>
  public string ConfigPath { get; set; } = "kind-config.yaml";

  /// <summary>
  /// The different Kustomizations to generate. First depends on the second, and so on.
  /// </summary>
  public IEnumerable<string> KustomizeFlows { get; set; } = ["apps", "infrastructure", "infrastructure/controllers"];

  /// <summary>
  /// The different places that it should be able to hook into the Kustomization flows. For example per cluster or distribution.
  /// </summary>
  public IEnumerable<string> KustomizeHooks { get; set; } = [];

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
}
