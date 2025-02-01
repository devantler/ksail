namespace KSail.Models.DeploymentTool;

/// <summary>
/// The source for reconciling GitOps resources.
/// </summary>
public interface IKSailGitOpsSource
{
  /// <summary>
  /// The URL of the GitOps source.
  /// </summary>
  Uri Url { get; set; }
}
