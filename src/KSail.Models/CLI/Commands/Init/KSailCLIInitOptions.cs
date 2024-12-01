namespace KSail.Models.CLI.Commands.Init;

/// <summary>
/// The options to use for the 'init' command.
/// </summary>
public class KSailCLIInitOptions
{
  /// <summary>
  /// Whether to generate a ksail-config.yaml file, to configure the KSail CLI declaratively.
  /// </summary>
  public bool DeclarativeConfig { get; set; } = true;

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
  /// The directory to place the generated output in.
  /// </summary>
  public string OutputDirectory { get; set; } = "./";

  /// <summary>
  /// The template to use for the generated output.
  /// </summary>
  public KSailCLIInitTemplate Template { get; set; } = KSailCLIInitTemplate.Simple;
}
