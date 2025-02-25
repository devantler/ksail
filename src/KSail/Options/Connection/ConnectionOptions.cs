using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Connection;

/// <summary>
/// Connection options.
/// </summary>
public class ConnectionOptions(KSailCluster config)
{
  /// <summary>
  /// The kube context to use for the connection.
  /// </summary>
  public readonly ConnectionContextOption ContextOption = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The kubeconfig to use for the connection.
  /// </summary>
  public readonly ConnectionKubeconfigOption KubeconfigOption = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The connection timeout.
  /// </summary>
  public readonly ConnectionTimeoutOption TimeoutOption = new(config) { Arity = ArgumentArity.ZeroOrOne };
}
