using System.ComponentModel;

namespace KSail.Models.DeploymentTool;

/// <summary>
/// The source for reconciling Flux resources.
/// </summary>
/// <remarks>
/// Constructs a new instance of the KSail OCI repository with the specified URL.
/// </remarks>
public class KSailOCIRepository : IKSailGitOpsSource
{
  /// <summary>
  /// The URL of the OCI repository.
  /// </summary>
  [Description("The URL of the OCI repository.")]
  public Uri Url { get; set; } = new Uri("oci://host.docker.internal:5555/ksail-registry");

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailOCIRepository"/> class.
  /// </summary>
  public KSailOCIRepository()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailOCIRepository"/> class with the specified URL.
  /// </summary>
  /// <param name="url"></param>
  public KSailOCIRepository(Uri url) => Url = url;
}
