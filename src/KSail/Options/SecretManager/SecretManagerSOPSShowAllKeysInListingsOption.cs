using System.CommandLine;
using KSail.Models;

namespace KSail.Options.SecretManager;

/// <summary>
/// Option to show all keys in the listed keys.
/// </summary>
public class SecretManagerSOPSShowAllKeysInListingsOption(KSailCluster config) : Option<bool?>(
  ["--all", "-a"],
  $"Show all keys. [default: {config.Spec.SecretManager.SOPS.ShowAllKeysInListings}]"
);
