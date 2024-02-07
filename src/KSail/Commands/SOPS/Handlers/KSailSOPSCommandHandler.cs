using KSail.CLIWrappers;
using KSail.Provisioners.SecretManager;

namespace KSail.Commands.SOPS.Handlers;

class KSailSOPSCommandHandler() : IDisposable
{
  readonly LocalSOPSProvisioner _localSOPSProvisioner = new();
  internal async Task<int> HandleAsync(string clusterName, bool generateKey, bool showPublicKey, bool showPrivateKey, string encrypt, string decrypt, string import, string export, CancellationToken token)
  {
    if (generateKey)
    {
      Console.WriteLine("🔐 Generating new SOPS key...");
      if (await AgeCLIWrapper.GenerateKeyAsync(clusterName, true, token) != 0)
      {
        Console.WriteLine("✕ SOPS key generation failed");
        return 1;
      }
      Console.WriteLine("✔ SOPS key generated");
    }
    else if (!string.IsNullOrWhiteSpace(encrypt))
    {
      Console.WriteLine($"🔐 Encrypting '{encrypt}'...");
      if (await SOPSCLIWrapper.EncryptAsync(encrypt, token) != 0)
      {
        Console.WriteLine("✕ SOPS encryption failed");
        return 1;
      }
      Console.WriteLine($"✔ '{encrypt}' encrypted");
    }
    else if (!string.IsNullOrWhiteSpace(decrypt))
    {
      Console.WriteLine($"🔐 Decrypting '{decrypt}'...");
      if (await SOPSCLIWrapper.DecryptAsync(decrypt, token) != 0)
      {
        Console.WriteLine("✕ SOPS decryption failed");
        return 1;
      }
      Console.WriteLine($"✔ '{decrypt}' decrypted");
    }
    else if (showPublicKey)
    {
      var (exitCode, publicKey) = await _localSOPSProvisioner.GetPublicKeyAsync(KeyType.Age, clusterName, token);
      if (exitCode != 0)
      {
        Console.WriteLine("✕ Public SOPS key not found");
        return 1;
      }
      Console.WriteLine(publicKey);
    }
    else if (showPrivateKey)
    {
      var (exitCode, privateKey) = await _localSOPSProvisioner.GetPrivateKeyAsync(KeyType.Age, clusterName, token);
      if (exitCode != 0)
      {
        Console.WriteLine("✕ Private SOPS key not found");
        return 1;
      }
      Console.WriteLine(privateKey);
    }
    else if (!string.IsNullOrWhiteSpace(import))
    {
      string? contents;
      if (File.Exists(import))
      {
        Console.WriteLine($"🔐 Importing SOPS key from '{import}'...");
        contents = await File.ReadAllTextAsync(import, token);
        Console.WriteLine($"✔ SOPS key imported from '{import}'");
      }
      else
      {
        Console.WriteLine("🔐 Importing SOPS key from stdin...");
        contents = import;
        Console.WriteLine("✔ SOPS key imported from stdin");
      }
      await File.WriteAllTextAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey"), contents, token);
    }
    else if (!string.IsNullOrWhiteSpace(export))
    {
      Console.WriteLine($"🔐 Exporting SOPS key to '{export}'...");
      if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey")))
      {
        Console.WriteLine("✕ SOPS key not found");
        return 1;
      }
      string contents = await File.ReadAllTextAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey"), token);
      await File.WriteAllTextAsync($"{export}/ksail_sops.agekey", contents, token);
      Console.WriteLine($"✔ SOPS key exported to '{export}'");
    }
    else
    {
      Console.WriteLine("✕ No option specified");
      return 1;
    }
    Console.WriteLine("");
    return 0;
  }

  public void Dispose()
  {
    _localSOPSProvisioner.Dispose();
    GC.SuppressFinalize(this);
  }
}
