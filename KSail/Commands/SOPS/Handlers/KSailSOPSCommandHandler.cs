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
      // case (true, false, false, false, "", "", "", ""):
      //   return await HandleGenerateKey(clusterName, token);
      // case (false, true, false, false, "", "", "", ""):
      //   return await HandleShowKey(clusterName, token);
      case (false, false, true, false, "", "", "", ""):
        return await HandleShowPublicKey(clusterName, token).ConfigureAwait(false);
      case (false, false, false, true, "", "", "", ""):
        return await HandleShowPrivateKey(clusterName, token).ConfigureAwait(false);
      case (false, false, false, false, not null, "", "", ""):
        return await HandleEncrypt(encrypt, clusterName, token).ConfigureAwait(false);
      case (false, false, false, false, "", not null, "", ""):
        return await HandleDecrypt(decrypt, clusterName, token).ConfigureAwait(false);
      case (false, false, false, false, "", "", not null, ""):
        return await HandleImport(clusterName, import, token).ConfigureAwait(false);
      case (false, false, false, false, "", "", "", not null):
        return await HandleExport(clusterName, export, token).ConfigureAwait(false);
      default:
        Console.WriteLine("✕ More than one option specified");
        return 1;
    }
  }

  //static async Task<int> HandleGenerateKey(string clusterName, CancellationToken token) => throw new NotImplementedException();

  //static async Task<int> HandleShowKey(string clusterName, CancellationToken token) => throw new NotImplementedException();

  async Task<int> HandleShowPrivateKey(string clusterName, CancellationToken token)
  {
    var (exitCode, privateKey) = await _localSOPSProvisioner.GetPrivateKeyAsync(KeyType.Age, clusterName, token).ConfigureAwait(false);
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
    var (exitCode, publicKey) = await _localSOPSProvisioner.GetPublicKeyAsync(KeyType.Age, clusterName, token).ConfigureAwait(false);
    if (exitCode != 0)
    {
      Console.WriteLine("✕ Public SOPS key not found");
      return 1;
    }
    Console.WriteLine(publicKey);
    return 0;
  }

  static async Task<int> HandleDecrypt(string decrypt, string clusterName, CancellationToken token)
  {
#pragma warning disable CA1308 // Normalize strings to uppercase
    clusterName = clusterName.ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
    Console.WriteLine($"🔐 Decrypting '{decrypt}'");
    string masterKeyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", $"{clusterName}.agekey");
    if (await SOPSCLIWrapper.DecryptAsync(decrypt, masterKeyPath, token).ConfigureAwait(false) != 0)
    {
      Console.WriteLine("✕ SOPS decryption failed");
      return 1;
    }
    Console.WriteLine($"✔ '{decrypt}' decrypted");
    return 0;
  }

  static async Task<int> HandleEncrypt(string encrypt, string clusterName, CancellationToken token)
  {
#pragma warning disable CA1308 // Normalize strings to uppercase
    clusterName = clusterName.ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
    Console.WriteLine($"🔐 Encrypting '{encrypt}'");
    string masterKeyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", $"{clusterName}.agekey");
    if (await SOPSCLIWrapper.EncryptAsync(encrypt, masterKeyPath, token).ConfigureAwait(false) != 0)
    {
      Console.WriteLine("✕ SOPS encryption failed");
      return 1;
    }
    Console.WriteLine($"✔ '{encrypt}' encrypted");
    return 0;
  }

  static async Task<int> HandleImport(string clusterName, string import, CancellationToken token)
  {
#pragma warning disable CA1308 // Normalize strings to uppercase
    clusterName = clusterName.ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
    string? contents;
    if (File.Exists(import))
    {
      Console.WriteLine($"🔐 Importing SOPS key from '{import}'");
      contents = await File.ReadAllTextAsync(import, token).ConfigureAwait(false);
    }
    else
    {
      Console.WriteLine("🔐 Importing SOPS key from stdin");
      contents = import;
    }
    if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", $"{clusterName}.agekey")))
    {
      _ = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age"));
    }
    await File.WriteAllTextAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", $"{clusterName}.agekey"), contents, token).ConfigureAwait(false);
    Console.WriteLine($"✔ SOPS key imported to '{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", $"{clusterName}.agekey")}'");
    return 0;
  }

  static async Task<int> HandleExport(string clusterName, string export, CancellationToken token)
  {
#pragma warning disable CA1308 // Normalize strings to uppercase
    clusterName = clusterName.ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
    Console.WriteLine($"🔐 Exporting SOPS key to '{export}'");
    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", $"{clusterName}.agekey")))
    {
      Console.WriteLine("✕ SOPS key not found");
      return 1;
    }
    string contents = await File.ReadAllTextAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "age", $"{clusterName}.agekey"), token).ConfigureAwait(false);
    await File.WriteAllTextAsync($"{export}/{clusterName}.agekey", contents, token).ConfigureAwait(false);
    Console.WriteLine($"✔ SOPS key exported to '{export}'");
    return 0;
  }

  public void Dispose()
  {
    _localSOPSProvisioner.Dispose();
    GC.SuppressFinalize(this);
  }
}
