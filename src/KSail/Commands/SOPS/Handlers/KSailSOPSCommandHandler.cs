using KSail.Provisioners.SecretManagement;

namespace KSail.Commands.SOPS.Handlers;

static class KSailSOPSCommandHandler
{
  static readonly SOPSProvisioner _sopsProvisioner = new();
  internal static async Task HandleAsync(bool showPublicKey, bool showPrivateKey)
  {
    if (!showPublicKey && !showPrivateKey)
    {
      Console.WriteLine("❌ No option selected...");
      Environment.Exit(1);
    }
    if (showPublicKey)
    {
      await _sopsProvisioner.ShowPublicKeyAsync();
    }
    if (showPrivateKey)
    {
      await _sopsProvisioner.ShowPrivateKeyAsync();
    }

    Console.WriteLine();
  }
}
