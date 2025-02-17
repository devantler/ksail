using Devantler.Keys.Age;
using Devantler.SecretManager.Core;

namespace KSail.Commands.Secrets.Handlers;

class KSailSecretsExportCommandHandler(string publicKey, string outputPath, ISecretManager<AgeKey> secretManager)
{
  readonly string _publicKey = publicKey;
  readonly string _outputPath = outputPath;
  readonly ISecretManager<AgeKey> _secretManager = secretManager;

  internal async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine($"► exporting '{_publicKey}' to '{_outputPath}'");
    var key = await _secretManager.GetKeyAsync(_publicKey, cancellationToken).ConfigureAwait(false);
    File.WriteAllText(_outputPath, key.ToString());
    Console.WriteLine("✔ key exported successfully");
    return 0;
  }
}
