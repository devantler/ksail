using System.Text.RegularExpressions;
using KSail.CLIWrappers;
using KSail.Provisioners.ContainerOrchestrator;
using KSail.Utils;

namespace KSail.Provisioners.SecretManagement;

sealed partial class SOPSProvisioner : ISecretManagementProvisioner, IDisposable
{
  [GeneratedRegex("export KSAIL_SOPS_PUBLIC_GPG_KEY='.*'")]
  private static partial Regex KSailSOPSPublicKeyFilter();

  [GeneratedRegex("export KSAIL_SOPS_PRIVATE_GPG_KEY='.*'")]
  private static partial Regex KSailSOPSPrivateKeyFilter();

  readonly KubernetesProvisioner _kubernetesProvisioner = new();

  public async Task CreateKeysAsync()
  {
    string? existingKey = Environment.GetEnvironmentVariable("KSAIL_SOPS_GPG_KEY");
    if (existingKey is not null && !string.IsNullOrWhiteSpace(existingKey))
    {
      Console.WriteLine("✅ Using existing SOPS GPG key from environment variable KSAIL_SOPS_GPG_KEY.");
    }
    else
    {
      Console.WriteLine("🔐🔑 Generating new SOPS GPG key and saving it to environment variable KSAIL_SOPS_GPG_KEY...");
      await GPGCLIWrapper.CreateGPGKeyAsync();
      Console.WriteLine("✅ SOPS GPG key generated successfully...");
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
      string privateKey = await GPGCLIWrapper.ExportPrivateKeyAsync(true);
      if (envFileContent.Contains("export KSAIL_SOPS_PRIVATE_GPG_KEY="))
      {
        envFileContent = KSailSOPSPrivateKeyFilter().Replace(envFileContent, $"export KSAIL_SOPS_PRIVATE_GPG_KEY='{privateKey}'");
      }
      else
      {
        //TODO: Add support for Fig, so that the env is appended in the right place
        envFileContent += $"\nexport KSAIL_SOPS_PRIVATE_GPG_KEY='{privateKey}'";
      }
      string publicKey = await GPGCLIWrapper.ExportPublicKeyAsync(true);
      if (envFileContent.Contains("export KSAIL_SOPS_PUBLIC_GPG_KEY="))
      {
        envFileContent = KSailSOPSPublicKeyFilter().Replace(envFileContent, $"export KSAIL_SOPS_PUBLIC_GPG_KEY='{publicKey}'");
      }
      else
      {
        //TODO: Add support for Fig, so that the env is appended in the right place
        envFileContent += $"\nexport KSAIL_SOPS_PUBLIC_GPG_KEY='{publicKey}'";
      }
      File.WriteAllText(envFilePath, envFileContent);
      Environment.SetEnvironmentVariable("KSAIL_SOPS_GPG_KEY", privateKey);
      Console.WriteLine("✅ SOPS GPG key saved to environment variable KSAIL_SOPS_GPG_KEY successfully...");
    }
  }

  public async Task ProvisionAsync()
  {
    await _kubernetesProvisioner.CreateSecretAsync("sops-gpg", new Dictionary<string, string>
    {
      ["sops.asc"] = Environment.GetEnvironmentVariable("KSAIL_SOPS_GPG_KEY") ??
        throw new InvalidOperationException("🚨 Could not find the SOPS GPG key in the KSAIL_SOPS_GPG_KEY environment variable.")
    }, "flux-system");
  }

  internal static async Task CreateSOPSConfigAsync(string manifestsPath)
  {
    Console.WriteLine("🔐📄 Creating SOPS config file...");
    string sopsConfigPath = $"{manifestsPath}/../.sops.yaml";
    string fingerprint = await GPGCLIWrapper.GetFingerprintAsync();
    string sopsConfigContent = $"""
      creation_rules:
        - path_regex: .sops.yaml
          encrypted_regex: ^(data|stringData)$
          pgp: ${fingerprint}
      """;

    File.WriteAllText(sopsConfigPath, sopsConfigContent);
    Console.WriteLine("✅ SOPS config file created successfully.");
  }

  public async Task ShowPublicKeyAsync()
  {
    string publicKey = Environment.GetEnvironmentVariable("KSAIL_SOPS_PUBLIC_GPG_KEY") ?? await GPGCLIWrapper.ExportPublicKeyAsync();
    Console.WriteLine($"🔐🔑 SOPS public key:\n{publicKey}");
  }

  public async Task ShowPrivateKeyAsync()
  {
    string privateKey = Environment.GetEnvironmentVariable("KSAIL_SOPS_PRIVATE_GPG_KEY") ?? await GPGCLIWrapper.ExportPrivateKeyAsync();
    if (ConsoleUtils.PromptLogin())
    {
      Console.WriteLine($"🔐🔑 SOPS private key:\n{privateKey}");
    }
  }

  public void Dispose()
  {
    _kubernetesProvisioner.Dispose();
    GC.SuppressFinalize(this);
  }
}
