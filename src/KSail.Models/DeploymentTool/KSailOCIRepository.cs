using System.ComponentModel;

namespace KSail.Models.DeploymentTool;

/// <summary>
/// The source for reconciling Flux resources.
/// </summary>
/// <remarks>
/// Constructs a new instance of the KSail repository with the specified URL.
/// </remarks>
public class KSailRepository
{
  /// <summary>
  /// The URL of the OCI repository.
  /// </summary>
  [Description("The URL of the repository.")]
  public Uri Url { get; set; } = new Uri("oci://host.docker.internal:5555/ksail-registry");

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailRepository"/> class.
  /// </summary>
  public KSailRepository()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailRepository"/> class with the specified URL.
  /// </summary>
  /// <param name="url"></param>
  public KSailRepository(Uri url) => Url = url;
}
