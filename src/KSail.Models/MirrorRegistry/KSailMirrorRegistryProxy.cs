using System.ComponentModel;

namespace KSail.Models.MirrorRegistry;

/// <summary>
/// An optional proxy for the registry to use to proxy and cache images.
/// </summary>
public class KSailMirrorRegistryProxy
{
  /// <summary>
  /// The URL of the upstream registry to proxy and cache images from.
  /// </summary>
  [Description("The URL of the upstream registry to proxy and cache images from.")]
  public required Uri Url { get; set; }

  /// <summary>
  /// The username to authenticate with the upstream registry.
  /// </summary>
  [Description("The username to authenticate with the upstream registry.")]
  public string? Username { get; set; }

  /// <summary>
  /// The password to authenticate with the upstream registry.
  /// </summary>
  [Description("The password to authenticate with the upstream registry.")]
  public string? Password { get; set; }

  /// <summary>
  /// Connect to the upstream registry over HTTPS.
  /// </summary>
  [Description("Connect to the upstream registry over HTTPS.")]
  public bool Insecure { get; set; }
}
