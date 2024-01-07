using KSail.Provisioners.Cluster;
using KSail.Utils;

namespace KSail.Commands.Down.Handlers;

static class KSailDownK3dCommandHandler
{
  static readonly K3dProvisioner _provisioner = new();

  internal static async Task HandleAsync(string name)
  {
    name ??= ConsoleUtils.Prompt("Please enter the name of the cluster to destroy");
    await _provisioner.DeprovisionAsync(name);
    Console.WriteLine();
  }
}
