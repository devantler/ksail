using System.ComponentModel;

namespace KSail.Models.Template;

/// <summary>
/// The options for the KSail Kustomize template.
/// </summary>
public class KSailKustomizeTemplateOptions
{
  /// <summary>
  /// The root kustomization file.
  /// </summary>
  [Description("The root kustomization file.")]
  public string RootKustomizationDir { get; set; } = "k8s/clusters/ksail-default/flux-system";

  /// <summary>
  /// The Flux kustomizations to include.
  /// </summary>
  [Description("The kustomizations to include. The first depends on the next, and so on.")]
  public string[] Kustomizations { get; set; } = ["apps", "infrastructure", "infrastructure/controllers"];

  /// <summary>
  /// The kustomize hooks to use.
  /// </summary>
  [Description("The kustomization hooks to use. Each kustomization hook includes an extension to the " +
    "kustomization allowing you to customize that kustomization at a specific point in the kustomize build process.")]
  public string[] KustomizationHooks { get; set; } = [];

  /// <summary>
  /// Enable components in the kustomize template.
  /// </summary>
  [Description("Enable components in the kustomize template.")]
  public bool Components { get; set; }
}
