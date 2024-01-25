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
      Console.WriteLine("🔐 SOPS public key (age):");
      Console.WriteLine(await SOPSProvisioner.GetPublicKeyAsync());
    }
    else if (showPrivateKey)
    {
      Console.WriteLine("🔐 SOPS private key (age):");
      Console.WriteLine(await SOPSProvisioner.GetPrivateKeyAsync());
    }
    else if (!string.IsNullOrWhiteSpace(import))
    {
      if (File.Exists(import))
      {
        Console.WriteLine($"🔐 Importing SOPS key from '{import}'...");
        // Read all contents of the file
        string contents = await File.ReadAllTextAsync(import);
        // Write the contents to 

        Console.WriteLine($"✔ SOPS key imported from '{import}'");
      }
      else
      {
        Console.WriteLine($"🔐 Importing SOPS key from stdin...");
        await SOPSCLIWrapper.ImportAsync(import);
        Console.WriteLine($"✔ SOPS key imported from stdin");
      }
    }
    else if (!string.IsNullOrWhiteSpace(export))
    {
      Console.WriteLine($"🔐 Exporting SOPS key to '{export}'...");
      await SOPSCLIWrapper.ExportAsync(export);
      Console.WriteLine($"✔ SOPS key exported to '{export}'");
    }
    else
    {
      throw new InvalidOperationException("You must specify either --generate-key, --show-public-key, --show-private-key, --encrypt or --decrypt");
    }
    Console.WriteLine("");
  }
}
