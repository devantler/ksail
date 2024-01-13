using KSail.CLIWrappers;

namespace KSail.Provisioners;

sealed class SOPSProvisioner : IProvisioner, IDisposable
{
  readonly KubernetesProvisioner kubernetesProvisioner = new();

  internal static async Task CreateKeysAsync()
  {
    if (File.Exists("ksail_sops.agekey"))
    {
      Console.WriteLine("✔ Using existing SOPS key...");
      return;
    }

    Console.WriteLine("► Generating new SOPS key...");
    await AgeKeygenCLIWrapper.GenerateKeyAsync();
  }

  public async Task ProvisionAsync()
  {
    string ageKey = await File.ReadAllTextAsync("ksail_sops.agekey");
    await kubernetesProvisioner.CreateSecretAsync("sops-gpg", new Dictionary<string, string>
    {
      ["sops.asc"] = ageKey
    }, "flux-system");
  }

  internal static Task CreateSOPSConfigAsync(string configPath) => throw new NotImplementedException();

  internal static Task ShowPublicKeyAsync() => throw new NotImplementedException();

  internal static Task ShowPrivateKeyAsync() => throw new NotImplementedException();

  public void Dispose()
  {
    kubernetesProvisioner.Dispose();
    GC.SuppressFinalize(this);
  }
}
