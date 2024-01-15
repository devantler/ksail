using System.CommandLine;
using KSail.CLIWrappers;
using KSail.Provisioners;

namespace KSail.Commands.SOPS.Handlers;

class KSailSOPSCommandHandler(IConsole console)
{
  readonly IConsole console = console;
  readonly AgeCLIWrapper ageCliWrapper = new(console);
  readonly SOPSCLIWrapper sopsCliWrapper = new(console);
  internal async Task HandleAsync(bool generateKey, bool showPublicKey, bool showPrivateKey, string encrypt, string decrypt)
  {
    if (generateKey)
    {
      console.WriteLine("🔐 Generating new SOPS key...");
      await ageCliWrapper.GenerateKeyAsync();
      console.WriteLine("✔ SOPS key generated");
    }
    else if (!string.IsNullOrWhiteSpace(encrypt))
    {
      console.WriteLine($"🔐 Encrypting '{encrypt}'...");
      await sopsCliWrapper.EncryptAsync(encrypt);
      console.WriteLine($"✔ '{encrypt}' encrypted");
    }
    else if (!string.IsNullOrWhiteSpace(decrypt))
    {
      console.WriteLine($"🔐 Decrypting '{decrypt}'...");
      await sopsCliWrapper.DecryptAsync(decrypt);
      console.WriteLine($"✔ '{decrypt}' decrypted");
    }
    else if (showPublicKey)
    {
      console.WriteLine("🔐 SOPS public key (age):");
      console.WriteLine(await SOPSProvisioner.GetPublicKeyAsync());
    }
    else if (showPrivateKey)
    {
      console.WriteLine("🔐 SOPS private key (age):");
      console.WriteLine(await SOPSProvisioner.GetPrivateKeyAsync());
    }
    else
    {
      throw new InvalidOperationException("You must specify either --generate-key, --show-public-key, --show-private-key, --encrypt or --decrypt");
    }
    console.WriteLine("");
  }
}
