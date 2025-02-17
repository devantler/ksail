using Devantler.Keys.Age;
using Devantler.SecretManager.Core;
using KSail.Models;

namespace KSail.Commands.Secrets.Handlers;

class KSailSecretsDeleteCommandHandler(KSailCluster config, string publicKey, ISecretManager<AgeKey> secretManager)
{
  readonly KSailCluster _config = config;
  readonly string _publicKey = publicKey;
  readonly ISecretManager<AgeKey> _secretManager = secretManager;

  internal async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine($"► deleting '{_publicKey}' from '{_config.Spec.Project.SecretManager}'");
    _ = await _secretManager.GetKeyAsync(_publicKey, cancellationToken).ConfigureAwait(false);
    Console.WriteLine($"✔ key deleted");
    return 0;
  }
}
