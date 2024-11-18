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
  public string KustomizationDirectory { get; set; } = "";

  /// <summary>
  /// The path to the distribution configuration file.
  /// </summary>
  public string ConfigPath { get; set; } = "kind-config.yaml";

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
