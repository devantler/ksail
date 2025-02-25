using System.CommandLine;
using KSail.Models;

namespace KSail.Options.DeploymentTool;

/// <summary>
/// Options for the Flux deployment tool.
/// </summary>
public class DeploymentToolFluxOptions(KSailCluster config)
{
  /// <summary>
  /// The source URL for the Flux deployment tool.
  /// </summary>
  public readonly DeploymentToolFluxSourceUrlOption SourceOption = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// Whether to enable Flux post-build variables.
  /// </summary>
  public readonly DeploymentToolFluxPostBuildVariablesOption PostBuildVariablesOption = new(config) { Arity = ArgumentArity.ZeroOrOne };
}
