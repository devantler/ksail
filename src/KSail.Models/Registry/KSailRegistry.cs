namespace KSail.Models.Registry;

/// <summary>
/// A registry to create for the KSail cluster to reconcile flux artifacts, and to proxy and cache images.
/// </summary>
public class KSailRegistry
{
  /// <summary>
  /// The name of the registry.
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  /// The host port of the registry (if applicable).
  /// </summary>
  public required int HostPort { get; set; }

  /// <summary>
  /// Whether this registry is the GitOps tool's OCI registry source for reconciliation.
  /// </summary>
  public bool IsGitOpsOCISource { get; set; }

  /// <summary>
  /// An optional proxy for the registry to use to proxy and cache images.
  /// </summary>
  public KSailRegistryProxy? Proxy { get; set; }

  /// <summary>
  /// The username to authenticate with the registry.
  /// </summary>
  public string? Username { get; set; }

  /// <summary>
  /// The password to authenticate with the registry.
  /// </summary>
  public string? Password { get; set; }


  /// <summary>
  /// The registry provider.
  /// </summary>
  public KSailRegistryProvider Provider { get; set; } = KSailRegistryProvider.Docker;
}
