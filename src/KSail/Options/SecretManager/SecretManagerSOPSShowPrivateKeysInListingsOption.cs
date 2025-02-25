using System.CommandLine;
using KSail.Models;

namespace KSail.Options.SecretManager;

/// <summary>
/// Option to show private keys in the listed keys.
/// </summary>
/// <param name="config"></param>
public class SecretManagerSOPSShowPrivateKeysInListingsOption(KSailCluster config) : Option<bool?>(
  ["--show-private-keys", "-spk"],
  $"Show private keys. [default: {config.Spec.SecretManager.SOPS.ShowPrivateKeysInListings}]"
)
{ }
