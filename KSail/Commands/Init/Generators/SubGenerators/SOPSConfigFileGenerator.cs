using Devantler.KeyManager.Core.Models;
using Devantler.KeyManager.Local.Age;
using Devantler.Keys.Age;
using KSail.Models;

namespace KSail.Commands.Init.Generators.SubGenerators;

class SOPSConfigFileGenerator
{
  LocalAgeKeyManager LocalAgeKeyManager { get; } = new();

  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    var ageKey = await LocalAgeKeyManager.CreateKeyAsync(cancellationToken).ConfigureAwait(false);
    string sopsConfigPath = Path.Combine(config.Spec.InitOptions.OutputDirectory, ".sops.yaml");
    if (!File.Exists(sopsConfigPath) || string.IsNullOrEmpty(await File.ReadAllTextAsync(sopsConfigPath, cancellationToken).ConfigureAwait(false)))
    {
      await GenerateNewSOPSConfigFile(sopsConfigPath, config.Metadata.Name, ageKey, cancellationToken).ConfigureAwait(false);
    }
    else
    {
      await GenerateUpdatedSOPSConfigFile(sopsConfigPath, config.Metadata.Name, ageKey, cancellationToken).ConfigureAwait(false);
    }
  }

  async Task GenerateNewSOPSConfigFile(string path, string clusterName, AgeKey ageKey, CancellationToken cancellationToken = default)
  {
    Console.WriteLine($"✚ Generating '{path}'");
    var sopsConfig = new SOPSConfig()
    {
      CreationRules =
        [
            new()
            {
              PathRegex = @$"^k8s\/clusters\/{clusterName}\/.+\.sops\.yaml$",
              Age = ageKey.PublicKey
            },
          new()
          {
            PathRegex = ".sops.yaml",
            Age = ageKey.PublicKey
          }
        ]
    };
    await LocalAgeKeyManager.CreateSOPSConfigAsync(path, sopsConfig, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateUpdatedSOPSConfigFile(string path, string clusterName, AgeKey ageKey, CancellationToken cancellationToken = default)
  {
    Console.WriteLine($"✚ Generating and overwriting '{path}'");
    var sopsConfig = await LocalAgeKeyManager.GetSOPSConfigAsync(path, cancellationToken: cancellationToken).ConfigureAwait(false);

    // Check if the creation rule already exists
    if (!sopsConfig.CreationRules.Any(cr => cr.PathRegex == @$"^k8s\/clusters\/{clusterName}\/.+\.sops\.yaml$"))
    {
      // Add new creation rule to the start of the list
      sopsConfig.CreationRules.Insert(0, new SOPSConfigCreationRule
      {
        PathRegex = @$"^k8s\/clusters\/{clusterName}\/.+\.sops\.yaml$",
        Age = ageKey.PublicKey
      });
      var lastCreationRule = sopsConfig.CreationRules.Last();
      string lastCreationRuleAge = lastCreationRule.Age;
      lastCreationRule.Age = $"{lastCreationRuleAge},{Environment.NewLine}{ageKey.PublicKey}";
      await LocalAgeKeyManager.CreateSOPSConfigAsync(path, sopsConfig, true, cancellationToken).ConfigureAwait(false);
    }
  }
}
