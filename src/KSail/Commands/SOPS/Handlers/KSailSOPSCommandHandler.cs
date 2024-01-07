using KSail.Provisioners.SecretManagement;
using KSail.Utils;

namespace KSail.Commands.SOPS.Handlers;

static class KSailSOPSCommandHandler
{
  static readonly SOPSProvisioner _sopsProvisioner = new();
  internal static async Task HandleAsync(bool showPublicKey, bool showPrivateKey)
  {
    if (!showPublicKey && !showPrivateKey)
    {
      if (bool.Parse(ConsoleUtils.Prompt("Show public key", "true", RegexFilters.YesNoFilter())))
      {
        showPublicKey = true;
      }
      else if (bool.Parse(ConsoleUtils.Prompt("Show private key", "false", RegexFilters.YesNoFilter())))
      {
        showPrivateKey = true;
      }
    }
    if (showPublicKey)
    {
      await _sopsProvisioner.ShowPublicKeyAsync();
    }
    else if (showPrivateKey)
    {
      await _sopsProvisioner.ShowPrivateKeyAsync();
    }
    else
    {
      Console.WriteLine("❌ No option selected...");
      Environment.Exit(1);
    }
    Console.WriteLine();
  }
}
