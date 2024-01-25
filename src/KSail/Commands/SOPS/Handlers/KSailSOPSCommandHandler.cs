using KSail.CLIWrappers;
using KSail.Provisioners;

namespace KSail.Commands.SOPS.Handlers;

class KSailSOPSCommandHandler()
{
  internal static async Task HandleAsync(bool generateKey, bool showPublicKey, bool showPrivateKey, string encrypt, string decrypt, string import, string export)
  {
    if (generateKey)
    {
      Console.WriteLine("🔐 Generating new SOPS key...");
      await AgeCLIWrapper.GenerateKeyAsync();
      Console.WriteLine("✔ SOPS key generated");
    }
    else if (!string.IsNullOrWhiteSpace(encrypt))
    {
      Console.WriteLine($"🔐 Encrypting '{encrypt}'...");
      await SOPSCLIWrapper.EncryptAsync(encrypt);
      Console.WriteLine($"✔ '{encrypt}' encrypted");
    }
    else if (!string.IsNullOrWhiteSpace(decrypt))
    {
      Console.WriteLine($"🔐 Decrypting '{decrypt}'...");
      await SOPSCLIWrapper.DecryptAsync(decrypt);
      Console.WriteLine($"✔ '{decrypt}' decrypted");
    }
    else if (showPublicKey)
    {
      Console.WriteLine(await SOPSProvisioner.GetPublicKeyAsync());
    }
    else if (showPrivateKey)
    {
      Console.WriteLine(await SOPSProvisioner.GetPrivateKeyAsync());
    }
    else if (!string.IsNullOrWhiteSpace(import))
    {
      string? contents;
      if (File.Exists(import))
      {
        Console.WriteLine($"🔐 Importing SOPS key from '{import}'...");
        contents = await File.ReadAllTextAsync(import);
        Console.WriteLine($"✔ SOPS key imported from '{import}'");
      }
      else
      {
        Console.WriteLine("🔐 Importing SOPS key from stdin...");
        contents = import;
        Console.WriteLine("✔ SOPS key imported from stdin");
      }
      await File.WriteAllTextAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey"), contents);
    }
    else if (!string.IsNullOrWhiteSpace(export))
    {
      Console.WriteLine($"🔐 Exporting SOPS key to '{export}'...");
      if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey")))
      {
        throw new FileNotFoundException("🚨 SOPS key not found");
      }
      string contents = await File.ReadAllTextAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ksail", "ksail_sops.agekey"));
      await File.WriteAllTextAsync($"{export}/ksail_sops.agekey", contents);
      Console.WriteLine($"✔ SOPS key exported to '{export}'");
    }
    else
    {
      throw new InvalidOperationException("No option specified");
    }
    Console.WriteLine("");
  }
}
