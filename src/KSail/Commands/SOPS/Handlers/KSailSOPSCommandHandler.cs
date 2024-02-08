using KSail.CLIWrappers;
using KSail.Provisioners.SecretManager;

namespace KSail.Commands.SOPS.Handlers;

class KSailSOPSCommandHandler() : IDisposable
{
  readonly LocalSOPSProvisioner _localSOPSProvisioner = new();
  internal async Task<int> HandleAsync(string clusterName, bool generateKey, bool showKey, bool showPublicKey, bool showPrivateKey, string encrypt, string decrypt, string import, string export, CancellationToken token)
  {
    switch (generateKey, showKey, showPublicKey, showPrivateKey, encrypt, decrypt, import, export)
    {
      case (true, false, false, false, "", "", "", ""):
        return await HandleGenerateKey(clusterName, token);
      case (false, true, false, false, "", "", "", ""):
        return await HandleShowKey(clusterName, token);
      case (false, false, true, false, "", "", "", ""):
        return await HandleShowPublicKey(clusterName, token);
      case (false, false, false, true, "", "", "", ""):
        return await HandleShowPrivateKey(clusterName, token);
      case (false, false, false, false, not null, "", "", ""):
        return await HandleEncrypt(encrypt, token);
      case (false, false, false, false, "", not null, "", ""):
        return await HandleDecrypt(decrypt, token);
      case (false, false, false, false, "", "", not null, ""):
        return await HandleImport(clusterName, import, token);
      case (false, false, false, false, "", "", "", not null):
        return await HandleExport(clusterName, export, token);
      default:
        Console.WriteLine("✕ More than one option specified");
        return 1;
    }
  }

  static async Task<int> HandleGenerateKey(string clusterName, CancellationToken token)
  {
    Console.WriteLine("🔐 Generating new SOPS key...");
    if (await AgeCLIWrapper.GenerateKeyAsync(clusterName, true, token) != 0)
    {
      Console.WriteLine("✕ SOPS key generation failed");
      return 1;
    }
    Console.WriteLine("✔ SOPS key generated");
    return 0;
  }

  static async Task<int> HandleShowKey(string clusterName, CancellationToken token) =>
    await AgeCLIWrapper.ShowKeyAsync(clusterName, token) != 0 ? 1 : 0;

  async Task<int> HandleShowPrivateKey(string clusterName, CancellationToken token)
  {
    var (exitCode, privateKey) = await _localSOPSProvisioner.GetPrivateKeyAsync(KeyType.Age, clusterName, token);
    if (exitCode != 0)
    {
      Console.WriteLine("✕ Private SOPS key not found");
      return 1;
    }
    Console.WriteLine(privateKey);
    return 0;
  }
  async Task<int> HandleShowPublicKey(string clusterName, CancellationToken token)
  {
    var (exitCode, publicKey) = await _localSOPSProvisioner.GetPublicKeyAsync(KeyType.Age, clusterName, token);
    if (exitCode != 0)
    {
      Console.WriteLine("✕ Public SOPS key not found");
      return 1;
    }
    Console.WriteLine(publicKey);
    return 0;
  }

  static async Task<int> HandleDecrypt(string decrypt, CancellationToken token)
  {
    Console.WriteLine($"🔐 Decrypting '{decrypt}'...");
    if (await SOPSCLIWrapper.DecryptAsync(decrypt, token) != 0)
    {
      Console.WriteLine("✕ SOPS decryption failed");
      return 1;
    }
    Console.WriteLine($"✔ '{decrypt}' decrypted");
    return 0;
  }

  static async Task<int> HandleEncrypt(string encrypt, CancellationToken token)
  {
    Console.WriteLine($"🔐 Encrypting '{encrypt}'...");
    if (await SOPSCLIWrapper.EncryptAsync(encrypt, token) != 0)
    {
      Console.WriteLine("✕ SOPS encryption failed");
      return 1;
    }
    Console.WriteLine($"✔ '{encrypt}' encrypted");
    return 0;
  }

  static async Task<int> HandleImport(string clusterName, string import, CancellationToken token)
  {
    clusterName = clusterName.ToLowerInvariant();
    string? contents;
    if (File.Exists(import))
    {
      Console.WriteLine($"🔐 Importing SOPS key from '{import}'...");
      contents = await File.ReadAllTextAsync(import, token);
    }
    else
    {
      Console.WriteLine("🔐 Importing SOPS key from stdin...");
      contents = import;
    }
    await File.WriteAllTextAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey"), contents, token);
    Console.WriteLine($"✔ SOPS key imported to '{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", $"{clusterName}.agekey")}'");
    return 0;
  }

  static async Task<int> HandleExport(string clusterName, string export, CancellationToken token)
  {
    clusterName = clusterName.ToLowerInvariant();
    Console.WriteLine($"🔐 Exporting SOPS key to '{export}'...");
    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", $"{clusterName}.agekey")))
    {
      Console.WriteLine("✕ SOPS key not found");
      return 1;
    }
    string contents = await File.ReadAllTextAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", $"{clusterName}.agekey"), token);
    await File.WriteAllTextAsync($"{export}/{clusterName}.agekey", contents, token);
    Console.WriteLine($"✔ SOPS key exported to '{export}'");
    return 0;
  }

  public void Dispose()
  {
    _localSOPSProvisioner.Dispose();
    GC.SuppressFinalize(this);
  }
}
