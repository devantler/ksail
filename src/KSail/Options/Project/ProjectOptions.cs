using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Project;

/// <summary>
/// Options for the project.
/// </summary>
/// <param name="config"></param>
public class ProjectOptions(KSailCluster config)
{
  /// <summary>
  /// The path to the ksail configuration file.
  /// </summary>
  public readonly ProjectConfigPathOption ConfigPathOption = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The project deployment tool.
  /// </summary>
  public readonly ProjectDeploymentToolOption DeploymentToolOption = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The path to the distribution configuration file.
  /// </summary>
  public readonly ProjectDistributionConfigPathOption DistributionConfigPathOption = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The project kubernetes distribution.
  /// </summary>
  public readonly ProjectDistributionOption DistributionOption = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The project editor.
  /// </summary>
  public readonly ProjectEditorOption EditorOption = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The project engine.
  /// </summary>
  public readonly ProjectEngineOption EngineOption = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// Enable mirror registries.
  /// </summary>
  public readonly ProjectMirrorRegistriesOption MirrorRegistriesOption = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The secret manager.
  /// </summary>
  public readonly ProjectSecretManagerOption SecretManagerOption = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The project template.
  /// </summary>
  public readonly ProjectTemplateOption TemplateOption = new(config) { Arity = ArgumentArity.ZeroOrOne };
}
