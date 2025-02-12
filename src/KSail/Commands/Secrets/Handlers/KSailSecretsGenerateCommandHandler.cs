using Devantler.Keys.Age;
using Devantler.SecretManager.Core;
using Devantler.SecretManager.SOPS.LocalAge;

class KSailSecretsGenerateCommandHandler()
{
  readonly ISecretManager<AgeKey> _secretManager = new SOPSLocalAgeSecretManager();

  internal async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    var key = await _secretManager.CreateKeyAsync(cancellationToken).ConfigureAwait(false);
    Console.WriteLine(key);
    return 0;
  }
}
