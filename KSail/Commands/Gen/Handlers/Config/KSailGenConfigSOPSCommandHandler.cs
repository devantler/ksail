using Devantler.KeyManager.Core.Models;
using Devantler.KeyManager.Local.Age;

namespace KSail.Commands.Gen.Handlers.Config;

class KSailGenConfigSopsCommandHandler
{
  readonly LocalAgeKeyManager _keyManager = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var sopsConfig = new SOPSConfig
    {
      CreationRules =
      [
        new() {
          PathRegex = @"^k8s\/.+\.sops\.yaml$",
          EncryptedRegex = "^(data | stringData)$",
          Age = """
          <age-public-key-1>
          """,
        }
      ]
    };
    await _keyManager.CreateSOPSConfigAsync(outputFile, sopsConfig, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
