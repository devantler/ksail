namespace KSail.Models;

/// <summary>
/// The options to use for the connection.
/// </summary>
public class KSailConnectionOptions
{
  /// <summary>
  /// The path to the kubeconfig file.
  /// </summary>
  public string Kubeconfig { get; set; } = $"~/.kube/config";

  /// <summary>
  /// The kube context.
  /// </summary>
  public string Context { get; set; } = "kind-default";

  /// <summary>
  /// The timeout for operations (in seconds).
  /// </summary>
  public string Timeout { get; set; } = "5m";
}
