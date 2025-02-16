using Devantler.Keys.Age;
using Devantler.SecretManager.Core;

namespace KSail.Commands.Secrets.Handlers;

class KSailSecretsEncryptCommandHandler(string path, ISecretManager<AgeKey> secretManager)
{
  readonly string _path = path;
  readonly ISecretManager<AgeKey> _secretManager = secretManager;

  internal async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine($"► encrypting '{_path}'");
    _ = await 
    Console.WriteLine($"✔ file encrypted successfully");
    return 0;
  }
}
