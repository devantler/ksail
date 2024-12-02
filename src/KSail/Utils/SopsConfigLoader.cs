using Devantler.KeyManager.Core.Models;
using Devantler.KeyManager.Local.Age;

namespace KSail.Utils;

static class SopsConfigLoader
{
  static readonly LocalAgeKeyManager _keyManager = new();
  internal static async Task<SOPSConfig> LoadAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine("► searching for a '.sops.yaml' file");
    string directory = Directory.GetCurrentDirectory();
    string sopsConfigPath = string.Empty;
    while (!string.IsNullOrEmpty(directory))
    {
      if (File.Exists(Path.Combine(directory, ".sops.yaml")))
      {
        sopsConfigPath = Path.Combine(directory, ".sops.yaml");
        Console.WriteLine($"✔ found '{sopsConfigPath}'");
        break;
      }
      directory = Directory.GetParent(directory)?.FullName ?? string.Empty;
    }
    if (string.IsNullOrEmpty(sopsConfigPath))
    {
      throw new KSailException("'.sops.yaml' file not found in the current or parent directories");
    }
    Console.WriteLine("► reading public key from '.sops.yaml' file");
    var sopsConfig = await _keyManager.GetSOPSConfigAsync(sopsConfigPath, cancellationToken).ConfigureAwait(false);
    return sopsConfig;
  }
}

