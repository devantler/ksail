using System.ComponentModel;

namespace KSail.Models.DeploymentTool;

/// <summary>
/// The source for reconciling Flux resources.
/// </summary>
public class KSailOCIRepository : IKSailGitOpsSource
{
  /// <summary>
  /// The URL of the OCI repository.
  /// </summary>
  [Description("The URL of the OCI repository.")]
  public Uri Url { get; set; } = new("https://host.docker.internal:5555/ksail-registry");
}
