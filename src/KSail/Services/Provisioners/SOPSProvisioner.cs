using KSail.CLIWrappers;
using KSail.Services.Provisioners.ContainerOrchestrator;

namespace KSail.Services.Provisioners;

sealed class SOPSProvisioner : IDisposable
{
  readonly KubernetesProvisioner _kubernetesProvisioner = new();

  internal static async Task CreateKeysAsync()
  {
    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.ksail/ksail_sops.agekey"))
    {
      Console.WriteLine("✔ Using existing SOPS key");
      return;
    }

    Console.WriteLine("► Generating new SOPS key...");
    await AgeCLIWrapper.GenerateKeyAsync();
  }

  public async Task ProvisionAsync()
  {
    string sopsKey = Environment.GetEnvironmentVariable("KSAIL_SOPS_KEY") ?? "";
    if (!string.IsNullOrEmpty(sopsKey))
    {
      Console.WriteLine("✔ Using SOPS key from KSAIL_SOPS_KEY");
      _ = Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.ksail");
      await File.WriteAllTextAsync(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.ksail/ksail_sops.agekey", sopsKey);
    }
    if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.ksail/ksail_sops.agekey"))
      throw new FileNotFoundException("🚨 SOPS key not found");
    string ageKey = await File.ReadAllTextAsync(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.ksail/ksail_sops.agekey");
    await _kubernetesProvisioner.CreateSecretAsync("sops-age", new Dictionary<string, string>
    {
      ["ksail_sops.agekey"] = ageKey
    }, "flux-system");
  }

  internal static async Task CreateSOPSConfigAsync(string configPath)
  {
    Console.WriteLine($"✚ Creating SOPS config '{configPath}'");
    string config = $"""
    creation_rules:
      - path_regex: .sops.yaml
        encrypted_regex: ^(data|stringData)$
        age: {await GetPublicKeyAsync()}
    """;
    await File.WriteAllTextAsync($"{configPath}", config);
  }

  internal static async Task<string> GetPublicKeyAsync()
  {
    string publicKeyLine = (await File.ReadAllLinesAsync($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/ksail_sops.agekey")).Skip(1).First();
    int startIndex = publicKeyLine.IndexOf("age", StringComparison.Ordinal);
    return publicKeyLine[startIndex..];
  }

  internal static async Task<string> GetPrivateKeyAsync() =>
    (await File.ReadAllLinesAsync($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/ksail_sops.agekey")).Last();

  public void Dispose()
  {
    _kubernetesProvisioner.Dispose();
    GC.SuppressFinalize(this);
  }
}
