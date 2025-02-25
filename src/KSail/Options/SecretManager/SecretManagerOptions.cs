using KSail.Models;

namespace KSail.Options.SecretManager;

/// <summary>
/// Options for the secret manager.
/// </summary>
/// <param name="config"></param>
public class SecretManagerOptions(KSailCluster config)
{
  /// <summary>
  /// Options for the SOPS secret manager.
  /// </summary>
  public readonly SecretManagerSOPSOptions SOPS = new(config);
}
