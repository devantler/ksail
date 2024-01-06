
using System.Runtime.InteropServices;
using KSail.CLIWrappers;

namespace KSail.Provisioners.SecretManagement;

/// <summary>
/// A provisioner for the SOPS secret management system.
/// </summary>
public class SOPSProvisioner : ISecretManagementProvisioner
{
  /// <summary>
  /// Creates the keys needed for encrypting and decrypting secrets.
  /// </summary>
  /// <exception cref="FileNotFoundException"></exception>
  /// <exception cref="PlatformNotSupportedException"></exception>
  public async Task CreateKeysAsync()
  {
    // If KSAIL_SOPS_GPG_KEY is set, use that key
    // Else, create a new key

    string? existingKey = Environment.GetEnvironmentVariable("KSAIL_SOPS_GPG_KEY");
    if (existingKey is not null && !string.IsNullOrWhiteSpace(existingKey))
    {
      Console.WriteLine("🔐🔑✅ Using existing SOPS GPG key from environment variable KSAIL_SOPS_GPG_KEY.");
    }
    else
    {
      Console.WriteLine("🔐🔑 Generating new SOPS GPG key...");
      string privateKey = await GPGCLIWrapper.CreateGPGKeyAsync();
      Console.WriteLine("🔐🔑✅ SOPS GPG key generated successfully...");
      Console.WriteLine("🔐🔑 Saving SOPS GPG key to environment variable KSAIL_SOPS_GPG_KEY...");
      //Write env variable to .zshrc
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
      File.WriteAllText(envFilePath, envFileContent);
      Console.WriteLine("🔐🔑✅ SOPS GPG key saved successfully...");
    }
  }

  /// <summary>
  /// Deploys the SOPS GPG key to the cluster.
  /// </summary>
  /// <exception cref="NotImplementedException"></exception>
  public Task DeploySecretManagementAsync() => throw new NotImplementedException();
}
