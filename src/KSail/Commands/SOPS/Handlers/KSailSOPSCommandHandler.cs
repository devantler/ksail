using KSail.CLIWrappers;
using KSail.Provisioners;

namespace KSail.Commands.SOPS.Handlers;

static class KSailSOPSCommandHandler
{
  internal static async Task HandleAsync(bool generateKey, bool showPublicKey, bool showPrivateKey, string encrypt, string decrypt)
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
    else
    {
      throw new InvalidOperationException("You must specify either --generate-key, --show-public-key, --show-private-key, --encrypt or --decrypt");
    }
    Console.WriteLine();
  }
}
