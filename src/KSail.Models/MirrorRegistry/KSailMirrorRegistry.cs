using System.ComponentModel;
using KSail.Models.Registry;

namespace KSail.Models.MirrorRegistry;

/// <summary>
/// A registry to create for the KSail cluster to reconcile flux artifacts, and to proxy and cache images.
/// </summary>
public class KSailMirrorRegistry : KSailRegistry
{
  /// <summary>
  /// An optional proxy for the registry to use to proxy and cache images.
  /// </summary>
  [Description("A proxy for the registry to use to proxy and cache images.")]
  public required KSailMirrorRegistryProxy Proxy { get; set; }
}
