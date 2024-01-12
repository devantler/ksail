using KSail.Provisioners.SecretManagement;

namespace KSail.Commands.SOPS.Handlers;

static class KSailSOPSCommandHandler
{
  internal static async Task HandleAsync(bool showPublicKey, bool showPrivateKey)
  {
    if (showPublicKey)
    {
      await SOPSProvisioner.ShowPublicKeyAsync();
    }
    if (showPrivateKey)
    {
      await SOPSProvisioner.ShowPrivateKeyAsync();
    }

    Console.WriteLine();
  }
}
