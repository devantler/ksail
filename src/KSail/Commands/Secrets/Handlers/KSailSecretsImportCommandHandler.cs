using Devantler.Keys.Age;
using Devantler.SecretManager.Core;
using KSail.Models;

namespace KSail.Commands.Secrets.Handlers;

class KSailSecretsImportCommandHandler(KSailCluster config, string key, ISecretManager<AgeKey> secretManager)
{
  readonly KSailCluster _config = config;
  readonly string _key = key;
  readonly ISecretManager<AgeKey> _secretManager = secretManager;

  internal async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine($"► importing '{_key}' to '{_config.Spec.Project.SecretManager}'");
    string key = File.ReadAllText(_key);
    if (File.Exists(key))
    {
      key = File.ReadAllText(key);
    }
    var ageKey = new AgeKey(key);
    _ = await _secretManager.ImportKeyAsync(ageKey, cancellationToken).ConfigureAwait(false);
    Console.WriteLine("✔ key imported");
    return 0;
  }
}
