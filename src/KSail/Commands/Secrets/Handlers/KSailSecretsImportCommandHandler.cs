using Devantler.Keys.Age;
using Devantler.SecretManager.Core;

namespace KSail.Commands.Secrets.Handlers;

class KSailSecretsImportCommandHandler(string key, ISecretManager<AgeKey> secretManager)
{
  readonly string _key = key;
  readonly ISecretManager<AgeKey> _secretManager = secretManager;

  internal async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine($"► importing '{_key}'");
    string key = File.ReadAllText(_key);
    if (File.Exists(key))
    {
      key = File.ReadAllText(key);
    }
    var ageKey = new AgeKey(key);
    _ = await _secretManager.ImportKeyAsync(ageKey, cancellationToken).ConfigureAwait(false);
    Console.WriteLine("✔ key imported successfully");
    return 0;
  }
}
