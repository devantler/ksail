using System.CommandLine;
using KSail.Models;

namespace KSail.Options.DeploymentTool;

/// <summary>
/// The source URL for the Flux deployment tool.
/// </summary>
public class DeploymentToolFluxSourceUrlOption(KSailCluster config) : Option<string>(
 ["--flux-source-url", "-fsu"],
  $"Flux source URL for reconciling GitOps resources. [default: {config.Spec.DeploymentTool.Flux.Source.Url}]"
);
