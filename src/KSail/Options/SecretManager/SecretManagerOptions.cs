using KSail.Models;

namespace KSail.Options.SecretManager;



internal class SecretManagerOptions(KSailCluster config)
{

  public readonly SecretManagerSOPSOptions SOPS = new(config);
}
