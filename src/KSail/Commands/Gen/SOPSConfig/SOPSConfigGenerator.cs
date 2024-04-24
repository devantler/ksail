using KSail.Models.K3d;
using KSail.Provisioners.SecretManager;
using KSail.TemplateEngine;

namespace KSail.Commands.Init.Generators;

class SOPSGenerator : IDisposable
{
  readonly Generator _generator = new(new TemplateEngine.TemplateEngine());
  readonly LocalSOPSProvisioner _localSOPSProvisioner = new();
  internal async Task GenerateSOPSConfigAsync(string manifestsDirectory, CancellationToken token)
  {
    var clusters = Directory.GetDirectories(Path.Combine(manifestsDirectory, "clusters")).Select(Path.GetFileName).ToList();
    var publicKeys = new List<string>();
    foreach (string? cluster in clusters)
    {
      if (string.IsNullOrEmpty(cluster))
        continue;
      var publicKey = await _localSOPSProvisioner.GetPublicKeyAsync(KeyType.Age, cluster, token);
      publicKeys.Add(publicKey.result);
    }
    await GenerateSOPSConfigAsync("./.sops.yaml", publicKeys);
  }

  Task GenerateSOPSConfigAsync(string filePath, List<string> publicKeys)
  {
    Console.WriteLine($"✚ Generating SOPS Config '{filePath}'");
    return _generator.GenerateAsync(
      filePath,
      $"{AppDomain.CurrentDomain.BaseDirectory}/assets/templates/sops/sops-config.sbn",
      new SOPSConfig
      {
        PublicKeys = publicKeys
      },
      FileMode.Create
    );
  }

  public void Dispose() => _localSOPSProvisioner.Dispose();
}
