using System.CommandLine;

namespace KSail.Options;

/// <summary>
/// Global options for the KSail CLI.
/// </summary>
public class GlobalOptions
{
  /// <summary>
  /// The name of the cluster.
  /// </summary>
  public MetadataNameOption MetadataNameOption = new() { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The distribution to use for the cluster.
  /// </summary>
  public ProjectDistributionOption ProjectDistributionOption = new() { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The engine to use for provisioning the cluster.
  /// </summary>
  public ProjectEngineOption ProjectEngineOption = new() { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// Enable mirror registries.
  /// </summary>
  public ProjectMirrorRegistriesOption ProjectMirrorRegistriesOption = new() { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The secret manager to use for the cluster.
  /// </summary>
  public ProjectSecretManagerOption ProjectSecretManagerOption = new() { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The path to the distribution configuration file.
  /// </summary>
  public PathOption ProjectDistributionConfigOption = new("Path to the distribution configuration file", ["--distribution-config", "-dc"]) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The working directory for the project.
  /// </summary>
  public PathOption ProjectWorkingDirectoryOption = new("The root directory of the project", ["--working-directory", "-wd"]) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The path to the ksail configuration file.
  /// </summary>
  public PathOption ProjectConfigOption = new("The path to the ksail configuration file", ["--ksail-config", "-kc"]) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The template to use for the project.
  /// </summary>
  public ProjectTemplateOption ProjectTemplateOption = new() { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The editor to use for the project.
  /// </summary>
  public ProjectEditorOption ProjectEditorOption = new() { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The kubeconfig to use for the connection.
  /// </summary>
  public ConnectionKubeconfigOption ConnectionKubeconfigOption = new() { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The kube context to use for the connection.
  /// </summary>
  public ConnectionContextOption ConnectionContextOption = new() { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The timeout for the connection.
  /// </summary>
  public ConnectionTimeoutOption ConnectionTimeoutOption = new() { Arity = ArgumentArity.ZeroOrOne };
}
