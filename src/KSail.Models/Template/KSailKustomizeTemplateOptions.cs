using System.ComponentModel;

namespace KSail.Models.Template;

/// <summary>
/// The options for the KSail Kustomize Template.
/// </summary>
public class KSailKustomizeTemplateOptions
{
  /// <summary>
  /// The root directory or kustomization file.
  /// </summary>
  [Description("The root directory.")]
  public string Root { get; set; } = "k8s/clusters/ksail-default/flux-system";

  /// <summary>
  /// The Flux kustomizations to include.
  /// </summary>
  [Description("The flows to include. The first depends on the next, and so on.")]
  public string[] Flows { get; set; } = ["apps", "infrastructure/configs", "infrastructure/controllers"];

  /// <summary>
  /// The kustomize hooks to use.
  /// </summary>
  [Description("The kustomization hooks to use. Each kustomization hook includes an extension to the " +
    "kustomization allowing you to customize that kustomization at a specific point in the kustomize build process.")]
  public string[] Hooks { get; set; } = [];
}
