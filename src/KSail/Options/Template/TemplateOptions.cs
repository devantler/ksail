using KSail.Models;

namespace KSail.Options.Template;

/// <summary>
/// Options for a template.
/// </summary>
/// <param name="config"></param>
public class TemplateOptions(KSailCluster config)
{
  /// <summary>
  /// Options for Kustomize.
  /// </summary>
  public readonly TemplateKustomizeOptions Kustomize = new(config);
}
