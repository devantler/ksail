using System.ComponentModel;

namespace KSail.Models.Registry;

/// <summary>
/// A registry to create for the KSail cluster to reconcile flux artifacts, and to proxy and cache images.
/// </summary>
public class KSailRegistry
{
  /// <summary>
  /// The name of the registry.
  /// </summary>
  [Description("The name of the registry.")]
  public required string Name { get; set; }

  /// <summary>
  /// The host port of the registry (if applicable).
  /// </summary>
  [Description("The host port of the registry (if applicable).")]
  public required int HostPort { get; set; }

  /// <summary>
  /// Whether this registry is the GitOps tool's source for reconciliation.
  /// </summary>
  [Description("Whether this registry is the GitOps tool's source for reconciliation.")]
  public bool IsGitOpsSource { get; set; }

  /// <summary>
  /// An optional proxy for the registry to use to proxy and cache images.
  /// </summary>
  [Description("An optional proxy for the registry to use to proxy and cache images.")]
  public KSailRegistryProxy? Proxy { get; set; }

  /// <summary>
  /// The username to authenticate with the registry.
  /// </summary>
  [Description("The username to authenticate with the registry.")]
  public string? Username { get; set; }

  /// <summary>
  /// The password to authenticate with the registry.
  /// </summary>
  [Description("The password to authenticate with the registry.")]
  public string? Password { get; set; }


  /// <summary>
  /// The registry provider.
  /// </summary>
  [Description("The registry provider.")]
  public KSailRegistryProvider Provider { get; set; } = KSailRegistryProvider.Docker;
}
