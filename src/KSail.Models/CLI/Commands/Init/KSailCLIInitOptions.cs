using System.ComponentModel;

namespace KSail.Models.CLI.Commands.Init;

/// <summary>
/// The options to use for the 'init' command.
/// </summary>
public class KSailCLIInitOptions
{
  /// <summary>
  /// Whether to generate a ksail-config.yaml file, to configure the KSail CLI declaratively.
  /// </summary>
  [Description("Whether to generate a ksail-config.yaml file, to configure the KSail CLI declaratively.")]
  public bool DeclarativeConfig { get; set; } = true;

  /// <summary>
  /// Whether to include Kustomize components in the generated output.
  /// </summary>
  [Description("Whether to include Kustomize components in the generated output.")]
  public bool Components { get; set; }

  /// <summary>
  /// Whether to include post build variables in the generated output (flux feature).
  /// </summary>
  [Description("Whether to include post build variables in the generated output (flux feature).")]
  public bool PostBuildVariables { get; set; }

  /// <summary>
  /// The directory to place the generated output in.
  /// </summary>
  [Description("The directory to place the generated output in.")]
  public string OutputDirectory { get; set; } = "./";

  /// <summary>
  /// The template to use for the generated output.
  /// </summary>
  [Description("The template to use for the generated output.")]
  public KSailCLIInitTemplate Template { get; set; } = KSailCLIInitTemplate.Simple;
}
