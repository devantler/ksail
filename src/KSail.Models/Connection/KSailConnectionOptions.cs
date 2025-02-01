using System.ComponentModel;

namespace KSail.Models.Connection;

/// <summary>
/// The options to use for the connection.
/// </summary>
public class KSailConnectionOptions
{
  /// <summary>
  /// The path to the kubeconfig file.
  /// </summary>
  [Description("The path to the kubeconfig file.")]
  public string Kubeconfig { get; set; } = "~/.kube/config";

  /// <summary>
  /// The kube context.
  /// </summary>
  [Description("The kube context.")]
  public string Context { get; set; } = "kind-ksail-default";

  /// <summary>
  /// The timeout for operations (30s, 5m, 1h).
  /// </summary>
  [Description("The timeout for operations (10s, 5m, 1h).")]
  public string Timeout { get; set; } = "5m";
}
