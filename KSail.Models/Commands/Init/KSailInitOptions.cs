namespace KSail.Models.Commands.Init;

/// <summary>
/// The options to use for the 'init' command.
/// </summary>
public class KSailInitOptions
{
  /// <summary>
  /// Whether to include Kustomize components in the generated output.
  /// </summary>
  public bool Components { get; set; }

  /// <summary>
  /// Whether to include Helm Releases in the generated output.
  /// </summary>
  public bool HelmReleases { get; set; }

  /// <summary>
  /// Whether to include post build variables in the generated output (flux feature).
  /// </summary>
  public bool PostBuildVariables { get; set; }

  /// <summary>
  /// The template to use for the generated output.
  /// </summary>
  public KSailInitTemplate Template { get; set; } = KSailInitTemplate.Simple;
}
