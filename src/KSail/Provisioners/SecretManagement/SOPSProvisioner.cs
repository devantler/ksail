using System.Text;
using System.Text.RegularExpressions;
using k8s;
using k8s.Models;
using KSail.CLIWrappers;

namespace KSail.Provisioners.SecretManagement;

/// <summary>
/// A provisioner for the SOPS secret management system.
/// </summary>
public partial class SOPSProvisioner : ISecretManagementProvisioner, IDisposable
{
  [GeneratedRegex("export KSAIL_SOPS_GPG_KEY=.*")]
  private static partial Regex KSailSOPSGPGFilter();

  readonly Kubernetes _kubernetesClient = new(KubernetesClientConfiguration.BuildDefaultConfig());

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
      Console.WriteLine("🔐🔑 Generating new SOPS GPG key and saving it to environment variable KSAIL_SOPS_GPG_KEY...");
      string privateKey = await GPGCLIWrapper.CreateGPGKeyAsync();
      Console.WriteLine("🔐🔑✅ SOPS GPG key generated successfully...");
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
      if (envFileContent.Contains("export KSAIL_SOPS_GPG_KEY="))
      {
        envFileContent = KSailSOPSGPGFilter().Replace(envFileContent, $"export KSAIL_SOPS_GPG_KEY='{privateKey}'");
      }
      File.WriteAllText(envFilePath, envFileContent);
      Environment.SetEnvironmentVariable("KSAIL_SOPS_GPG_KEY", privateKey);
      Console.WriteLine("🔐🔑✅ SOPS GPG key saved to environment variable KSAIL_SOPS_GPG_KEY successfully...");
    }
  }

  /// <summary>
  /// Deploys the SOPS GPG key to the cluster.
  /// </summary>
  /// <exception cref="NotImplementedException"></exception>
  /// <exception cref="InvalidOperationException"></exception>
  public async Task DeploySecretManagementAsync()
  {
    Console.WriteLine("🔐🚀 Deploying SOPS GPG key to cluster...");
    var sopsGpgSecret = new V1Secret
    {
      ApiVersion = "v1",
      Kind = "Secret",
      Metadata = new V1ObjectMeta
      {
        Name = "sops-gpg",
        NamespaceProperty = "flux-system"
      },
      Type = "Opaque",
      Data = new Dictionary<string, byte[]>
      {
        ["sops.asc"] = Encoding.UTF8.GetBytes(
          Environment.GetEnvironmentVariable("KSAIL_SOPS_GPG_KEY") ??
            throw new InvalidOperationException("🚨 Could not find the SOPS GPG key in the KSAIL_SOPS_GPG_KEY environment variable.")
          )
      }
    };
    _ = await _kubernetesClient.CreateNamespacedSecretAsync(sopsGpgSecret, "flux-system");
    Console.WriteLine("🔐🚀✅ SOPS GPG key deployed to cluster successfully...");
  }
  /// <inheritdoc/>
  public void Dispose()
  {
    _kubernetesClient.Dispose();
    GC.SuppressFinalize(this);
  }
}
