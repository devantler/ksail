using Devantler.Keys.Age;
using Devantler.SecretManager.Core;
using Devantler.SecretManager.SOPS.LocalAge;

class KSailSecretsDeleteCommandHandler(string publicKey)
{
  readonly string _publicKey = publicKey;
  readonly ISecretManager<AgeKey> _secretManager = new SOPSLocalAgeSecretManager();

  internal async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine($"► deleting '{_publicKey}'");
    var key = await _secretManager.DeleteKeyAsync(_publicKey, cancellationToken).ConfigureAwait(false);
    Console.WriteLine($"✔ removed '{key.PublicKey}'");
    return 0;
  }
}
