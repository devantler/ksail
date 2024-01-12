using KSail.Provisioners.Cluster;

namespace KSail.Commands.Down.Handlers;

static class KSailDownCommandHandler
{
  static readonly K3dProvisioner provisioner = new();

  internal static async Task HandleAsync(string name)
  {
    await provisioner.DeprovisionAsync(name);
    Console.WriteLine();
  }
}
