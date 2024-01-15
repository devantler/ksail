using KSail.CLIWrappers;

namespace KSail.Provisioners;

sealed class SOPSProvisioner : IProvisioner, IDisposable
{
  readonly KubernetesProvisioner kubernetesProvisioner = new();

  internal static async Task CreateKeysAsync()
  {
    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.ksail/ksail_sops.agekey"))
    {
      console.WriteLine("✔ Using existing SOPS key");
      return;
    }

    console.WriteLine("► Generating new SOPS key...");
    await AgeCLIWrapper.GenerateKeyAsync();
  }

  public async Task ProvisionAsync()
  {
    string ageKey = await File.ReadAllTextAsync(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.ksail/ksail_sops.agekey");
    await kubernetesProvisioner.CreateSecretAsync("sops-age", new Dictionary<string, string>
    {
      ["ksail_sops.agekey"] = ageKey
    }, "flux-system");
  }

  internal static async Task CreateSOPSConfigAsync(string configPath)
  {
    console.WriteLine($"► Creating SOPS config '{configPath}'");
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
    kubernetesProvisioner.Dispose();
    GC.SuppressFinalize(this);
  }
}
