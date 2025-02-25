namespace KSail.Models.Template;

/// <summary>
/// Options for the KSail Kustomize Template.
/// </summary>
public class KSailTemplate
{
  /// <summary>
  /// The Kustomize template.
  /// </summary>
  public KSailTemplateKustomize Kustomize { get; set; } = new KSailTemplateKustomize();
}
