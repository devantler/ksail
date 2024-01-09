using System.Text.RegularExpressions;
using KSail.CLIWrappers;
using KSail.Provisioners.ContainerOrchestrator;

namespace KSail.Provisioners.SecretManagement;

sealed partial class SOPSProvisioner : ISecretManagementProvisioner, IDisposable
{
  [GeneratedRegex("export KSAIL_SOPS_PUBLIC_GPG_KEY='.*'")]
  private static partial Regex KSailSOPSPublicKeyFilter();

  [GeneratedRegex("export KSAIL_SOPS_PRIVATE_GPG_KEY='.*'")]
  private static partial Regex KSailSOPSPrivateKeyFilter();

  readonly KubernetesProvisioner kubernetesProvisioner = new();

  public async Task CreateKeysAsync()
  {
    string? existingKey = Environment.GetEnvironmentVariable("KSAIL_SOPS_PRIVATE_GPG_KEY");
    if (!string.IsNullOrWhiteSpace(existingKey))
    {
      Console.WriteLine("✔ Using existing SOPS GPG key from environment variables...");
      return;
    }

    Console.WriteLine("► Generating new SOPS GPG key and saving it to environment variables...");
    await GPGCLIWrapper.CreateGPGKeyAsync();

    string envFilePath = GetEnvironmentFilePath();
    string envFileContent = File.ReadAllText(envFilePath);

    string privateKey = await GPGCLIWrapper.ExportPrivateKeyAsync(true);
    string publicKey = await GPGCLIWrapper.ExportPublicKeyAsync(true);
    const string figPostBlock = "# Fig post block. Keep at the bottom of this file.";
    string privateKeyExportStatement = $"export KSAIL_SOPS_PRIVATE_GPG_KEY='{privateKey}'";
    string publicKeyExportStatement = $"export KSAIL_SOPS_PUBLIC_GPG_KEY='{publicKey}'";

    envFileContent = ReplaceOrInsertExportStatement(envFileContent, privateKeyExportStatement, KSailSOPSPrivateKeyFilter(), figPostBlock);
    envFileContent = ReplaceOrInsertExportStatement(envFileContent, publicKeyExportStatement, KSailSOPSPublicKeyFilter(), figPostBlock);

    File.WriteAllText(envFilePath, envFileContent);
    Environment.SetEnvironmentVariable("KSAIL_SOPS_PRIVATE_GPG_KEY", privateKey);
    Environment.SetEnvironmentVariable("KSAIL_SOPS_PUBLIC_GPG_KEY", publicKey);
  }

  public async Task ProvisionAsync()
  {
    await kubernetesProvisioner.CreateSecretAsync("sops-gpg", new Dictionary<string, string>
    {
      ["sops.asc"] = Environment.GetEnvironmentVariable("KSAIL_SOPS_PRIVATE_GPG_KEY") ??
        throw new InvalidOperationException("🚨 Could not find the SOPS GPG key in the KSAIl_SOPS_PRIVATE_GPG_KEY environment variable.")
    }, "flux-system");
  }

  internal static async Task CreateSOPSConfigAsync(string manifestsPath)
  {
    Console.WriteLine("► Creating SOPS config file...");
    string sopsConfigPath = $"{manifestsPath}/../.sops.yaml";
    string fingerprint = await GPGCLIWrapper.GetFingerprintAsync();
    string sopsConfigContent = $"""
      creation_rules:
        - path_regex: .sops.yaml
          encrypted_regex: ^(data|stringData)$
          pgp: ${fingerprint}
      """;

    await File.WriteAllTextAsync(sopsConfigPath, sopsConfigContent);
  }

  public async Task ShowPublicKeyAsync()
  {
    string publicKey = Environment.GetEnvironmentVariable("KSAIL_SOPS_PUBLIC_GPG_KEY") ?? await GPGCLIWrapper.ExportPublicKeyAsync(true);
    Console.WriteLine(publicKey);
    Console.WriteLine();
  }

  public async Task ShowPrivateKeyAsync()
  {
    string privateKey = Environment.GetEnvironmentVariable("KSAIL_SOPS_PRIVATE_GPG_KEY") ?? await GPGCLIWrapper.ExportPrivateKeyAsync(true);
    Console.WriteLine(privateKey);
    Console.WriteLine();
  }

  static string GetEnvironmentFilePath()
  {
    string envFilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    if (File.Exists($"{envFilePath}/.zshrc"))
    {
      return $"{envFilePath}/.zshrc";
    }
    else if (File.Exists($"{envFilePath}/.bashrc"))
    {
      return $"{envFilePath}/.bashrc";
    }

    throw new FileNotFoundException("🚨 Could not save SOPS GPG key to environment variables because neither .zshrc nor .bashrc were found in the user's home directory.");
  }

  static string ReplaceOrInsertExportStatement(string envFileContent, string exportStatement, Regex filterRegex, string insertMarker)
  {
    if (envFileContent.Contains(exportStatement))
    {
      return filterRegex.Replace(envFileContent, exportStatement);
    }
    else if (envFileContent.Contains(insertMarker))
    {
      int insertIndex = envFileContent.IndexOf(insertMarker, StringComparison.InvariantCulture);
      return envFileContent.Insert(insertIndex, $"{exportStatement}\n");
    }
    else
    {
      return $"{envFileContent}\n{exportStatement}";
    }
  }

  public void Dispose()
  {
    kubernetesProvisioner.Dispose();
    GC.SuppressFinalize(this);
  }
}
