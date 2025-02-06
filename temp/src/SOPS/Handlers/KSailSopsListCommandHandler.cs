using Devantler.SecretManager.SOPS.LocalAge;
using Devantler.Keys.Age;
using KSail.Models;
using KSail.Utils;

namespace KSail.Commands.SOPS.Handlers;

class KSailSOPSListCommandHandler(KSailCluster config)
{
  readonly KSailCluster _config = config;
  readonly LocalAgeSecretManager _secretManager = new();

  internal async Task<bool> HandleAsync(CancellationToken cancellationToken)
  {
    var keys = await _secretManager.ListKeysAsync(cancellationToken).ConfigureAwait(false);

    if (_config.Spec.CLI.SopsOptions.List.ShowSOPSConfigKeysOnly)
    {
      var sopsConfig = await SopsConfigLoader.LoadAsync(cancellationToken).ConfigureAwait(false);
      if (!keys.Any(key => sopsConfig.CreationRules.Any(rule => rule.Age == key.PublicKey)))
      {
        Console.WriteLine("No keys found");
        return true;
      }
      foreach (var key in keys.Where(key => sopsConfig.CreationRules.Any(rule => rule.Age == key.PublicKey)))
      {
        if (_config.Spec.CLI.SopsOptions.List.ShowPrivateKey)
        {
          Console.WriteLine(key);
        }
        else
        {
          Console.WriteLine(Obscure(key));
        }
        Console.WriteLine();
      }
    }
    else
    {
      if (!keys.Any())
      {
        Console.WriteLine("No keys found");
        return true;
      }
      foreach (var key in keys)
      {

        if (_config.Spec.CLI.SopsOptions.List.ShowPrivateKey)
        {
          Console.WriteLine(key);
        }
        else
        {
          Console.WriteLine(Obscure(key));
        }
        Console.WriteLine();
      }
    }
    return true;
  }

  static string Obscure(AgeKey key)
  {
    string keyString = key.ToString();
    keyString = keyString.Replace(keyString[keyString.LastIndexOf('\n')..], "\nAGE-SECRET-KEY-" + new string('*', 59));
    return keyString;
  }
}
