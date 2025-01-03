using System.ComponentModel;

namespace KSail.Models.Registry;

/// <summary>
/// A registry to create for the KSail cluster to reconcile flux artifacts, and to proxy and cache images.
/// </summary>
public class KSailMirrorRegistry : KSailRegistry
{
  /// <summary>
  /// An optional proxy for the registry to use to proxy and cache images.
  /// </summary>
  [Description("A proxy for the registry to use to proxy and cache images.")]
  public required KSailRegistryProxy Proxy { get; set; }

  /// <summary>
  /// The host port of the registry.
  /// </summary>
  [Description("The host port of the registry (if applicable).")]
  public new int HostPort { get; set; }
}
