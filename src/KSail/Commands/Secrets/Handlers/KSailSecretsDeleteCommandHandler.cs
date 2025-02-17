using Devantler.Keys.Age;
using Devantler.SecretManager.Core;

namespace KSail.Commands.Secrets.Handlers;

class KSailSecretsDeleteCommandHandler(string publicKey, ISecretManager<AgeKey> secretManager)
{
  readonly string _publicKey = publicKey;
  readonly ISecretManager<AgeKey> _secretManager = secretManager;

  internal async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine($"► deleting '{_publicKey}'");
    _ = await _secretManager.GetKeyAsync(_publicKey, cancellationToken).ConfigureAwait(false);
    Console.WriteLine($"✔ key removed successfully");
    return 0;
  }
}
