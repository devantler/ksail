using KSail.Provisioners.SecretManagement;

namespace KSail.Commands.SOPS.Handlers;

static class KSailSOPSCommandHandler
{
  static readonly SOPSProvisioner sopsProvisioner = new();
  internal static async Task HandleAsync(bool showPublicKey, bool showPrivateKey)
  {
    if (showPublicKey)
    {
      await sopsProvisioner.ShowPublicKeyAsync();
    }
    if (showPrivateKey)
    {
      await sopsProvisioner.ShowPrivateKeyAsync();
    }

    Console.WriteLine();
  }
}
