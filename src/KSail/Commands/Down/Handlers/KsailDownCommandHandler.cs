using KSail.Provisioners;

namespace KSail.Commands.Down.Handlers;

internal class KSailDownCommandHandler()
{
  internal static async Task HandleAsync(string name)
  {
    await K3dProvisioner.DeprovisionAsync(name);
    Console.WriteLine();
  }
}
