using Devantler.Keys.Age;
using Devantler.SecretManager.Core;

namespace KSail.Commands.Secrets.Handlers;

class KSailSecretsEncryptCommandHandler(string path, string? publicKey, bool inPlace, string? output, ISecretManager<AgeKey> secretManager)
{
  readonly string _path = path;
  readonly string? _publicKey = publicKey;
  readonly bool _inPlace = inPlace;
  readonly string? _output = output;
  readonly ISecretManager<AgeKey> _secretManager = secretManager;

  internal async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    string encrypted = await _secretManager.EncryptAsync(_path, _publicKey, cancellationToken).ConfigureAwait(false);
    if (_inPlace)
    {
      await File.WriteAllTextAsync(_path, encrypted, cancellationToken);
    }
    if (!string.IsNullOrEmpty(_output))
    {
      await File.WriteAllTextAsync(_output, encrypted, cancellationToken);
    }

    return 0;
  }
}
