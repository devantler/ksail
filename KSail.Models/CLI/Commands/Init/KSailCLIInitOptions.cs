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

  /// <summary>
  /// The different Kustomizations to generate. First depends on the second, and so on.
  /// </summary>
  public IEnumerable<string> KustomizeFlows { get; set; } = ["apps", "infrastructure", "infrastructure/controllers"];

  /// <summary>
  /// The different places that it should be able to hook into the Kustomization flows. For example per cluster or distribution.
  /// </summary>
  public IEnumerable<string> KustomizeHooks { get; set; } = [];
}