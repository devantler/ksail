using System.ComponentModel;

namespace KSail.Models.Project;

/// <summary>
/// The options for the KSail project.
/// </summary>
public class KSailProjectOptions
{
  /// <summary>
  /// The path to the directory that contains the manifests.
  /// </summary>
  [Description("The path to the directory that contains the manifests.")]
  public string ManifestsDirectory { get; set; } = "./k8s";

  /// <summary>
  /// The relative path to the directory that contains the root kustomization file.
  /// </summary>
  [Description("The relative path to the directory that contains the root kustomization file.")]
  public string KustomizationDirectory { get; set; } = "./k8s/clusters/ksail-default/flux-system";

  /// <summary>
  /// The path to the distribution configuration file.
  /// </summary>
  [Description("The path to the distribution configuration file.")]
  public string ConfigPath { get; set; } = "kind-config.yaml";

  /// <summary>
  /// The different Kustomizations to generate. First depends on the second, and so on.
  /// </summary>
  [Description("The different Kustomizations to generate. First depends on the second, and so on.")]
  public IEnumerable<string> KustomizeFlows { get; set; } = ["apps", "infrastructure", "infrastructure/controllers"];

  /// <summary>
  /// The different places that it should be able to hook into the Kustomization flows. For example per cluster or distribution.
  /// </summary>
  [Description("The different places that it should be able to hook into the Kustomization flows. For example per cluster or distribution.")]
  public IEnumerable<string> KustomizeHooks { get; set; } = [];

  /// <summary>
  /// The Kubernetes distribution to use.
  /// </summary>
  [Description("The Kubernetes distribution to use.")]
  public KSailKubernetesDistribution Distribution { get; set; } = KSailKubernetesDistribution.Kind;

  /// <summary>
  /// The GitOps tool to use.
  /// </summary>
  [Description("The GitOps tool to use.")]
  public KSailGitOpsTool GitOpsTool { get; set; } = KSailGitOpsTool.Flux;

  /// <summary>
  /// The container engine to use.
  /// </summary>
  [Description("The container engine to use.")]
  public KSailContainerEngine ContainerEngine { get; set; } = KSailContainerEngine.Docker;

  /// <summary>
  /// Whether to enable SOPS support.
  /// </summary>
  [Description("Whether to enable SOPS support.")]
  public bool Sops { get; set; }
}
