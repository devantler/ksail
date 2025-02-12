using Devantler.Keys.Age;
using Devantler.SecretManager.Core;

class KSailSecretsDeleteCommandHandler(string publicKey, ISecretManager<AgeKey> secretManager)
{
  readonly string _publicKey = publicKey;
  readonly ISecretManager<AgeKey> _secretManager = secretManager;

  internal async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine($"► deleting '{_publicKey}'");
    var key = await _secretManager.DeleteKeyAsync(_publicKey, cancellationToken).ConfigureAwait(false);
    Console.WriteLine($"✔ removed '{key.PublicKey}'");
    return 0;
  }
}
