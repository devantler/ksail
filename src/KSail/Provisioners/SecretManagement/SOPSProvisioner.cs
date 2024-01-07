using System.Text.RegularExpressions;
using KSail.CLIWrappers;
using KSail.Provisioners.ContainerOrchestrator;

namespace KSail.Provisioners.SecretManagement;

sealed partial class SOPSProvisioner : ISecretManagementProvisioner, IDisposable
{
  [GeneratedRegex("export KSAIL_SOPS_GPG_KEY=.*")]
  private static partial Regex KSailSOPSGPGFilter();

  readonly KubernetesProvisioner _kubernetesProvisioner = new();

  public async Task CreateKeysAsync()
  {
    string? existingKey = Environment.GetEnvironmentVariable("KSAIL_SOPS_GPG_KEY");
    if (existingKey is not null && !string.IsNullOrWhiteSpace(existingKey))
    {
      Console.WriteLine("🔐🔑✅ Using existing SOPS GPG key from environment variable KSAIL_SOPS_GPG_KEY.");
    }
    else
    {
      Console.WriteLine("🔐🔑 Generating new SOPS GPG key and saving it to environment variable KSAIL_SOPS_GPG_KEY...");
      string privateKey = await GPGCLIWrapper.CreateGPGKeyAsync();
      Console.WriteLine("🔐🔑✅ SOPS GPG key generated successfully...");
      string envFilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
      if (File.Exists($"{envFilePath}/.zshrc"))
      {
        envFilePath += "/.zshrc";
      }
      else if (File.Exists($"{envFilePath}/.bashrc"))
      {
        envFilePath += "/.bashrc";
      }
      else
      {
        throw new FileNotFoundException("🚨 Could not save SOPS GPG key to environment variable KSAIL_SOPS_GPG_KEY because neither .zshrc nor .bashrc were found in the user's home directory.");
      }
      string envFileContent = File.ReadAllText(envFilePath);
      envFileContent += $"\nexport KSAIL_SOPS_GPG_KEY='{privateKey}'";
      if (envFileContent.Contains("export KSAIL_SOPS_GPG_KEY="))
      {
        envFileContent = KSailSOPSGPGFilter().Replace(envFileContent, $"export KSAIL_SOPS_GPG_KEY='{privateKey}'");
      }
      File.WriteAllText(envFilePath, envFileContent);
      Environment.SetEnvironmentVariable("KSAIL_SOPS_GPG_KEY", privateKey);
      Console.WriteLine("🔐🔑✅ SOPS GPG key saved to environment variable KSAIL_SOPS_GPG_KEY successfully...");
    }
  }

  public async Task DeploySecretManagementAsync()
  {
    await _kubernetesProvisioner.CreateSecretAsync("sops-gpg", new Dictionary<string, string>
    {
      ["sops.asc"] = Environment.GetEnvironmentVariable("KSAIL_SOPS_GPG_KEY") ??
        throw new InvalidOperationException("🚨 Could not find the SOPS GPG key in the KSAIL_SOPS_GPG_KEY environment variable.")
    }, "flux-system");
  }

  public void Dispose()
  {
    _kubernetesProvisioner.Dispose();
    GC.SuppressFinalize(this);
  }
}
