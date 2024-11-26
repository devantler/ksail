using Devantler.KeyManager.Local.Age;
using KSail.Models;

namespace KSail.Commands.SOPS.Handlers;

class KSailSOPSListCommandHandler(KSailCluster config)
{
  readonly KSailCluster _config = config;
  readonly LocalAgeKeyManager _keyManager = new();

  internal async Task<bool> HandleAsync(CancellationToken cancellationToken)
  {
    var keys = await _keyManager.ListKeysAsync(cancellationToken).ConfigureAwait(false);

    if (!keys.Any())
    {
      Console.WriteLine("No keys found");
      return false;
    }
    else
    {
      foreach (var key in keys)
      {
        if (_config.Spec.CLI.SopsOptions.ListOptions.ShowPrivateKey)
        {
          Console.WriteLine(key);
        }
        else
        {
          string keyString = key.ToString();
          keyString = keyString.Replace(keyString[keyString.LastIndexOf('\n')..], "\nAGE-SECRET-KEY-" + new string('*', 59));
          Console.WriteLine(keyString);
        }
        Console.WriteLine();
      }
    }
    return true;
  }
}
