using Devantler.Keys.Age;
using Devantler.SecretManager.Core;

class KSailSecretsImportCommandHandler(string inputPath, ISecretManager<AgeKey> secretManager)
{
  readonly string _inputPath = inputPath;
  readonly ISecretManager<AgeKey> _secretManager = secretManager;

  internal async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine($"► importing key from '{_inputPath}'");
    string key = File.ReadAllText(_inputPath);
    var ageKey = new AgeKey(key);
    _ = await _secretManager.ImportKeyAsync(ageKey, cancellationToken).ConfigureAwait(false);
    Console.WriteLine("✔ key imported successfully");
    return 0;
  }
}
