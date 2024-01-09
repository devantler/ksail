using KSail.Provisioners.Cluster;

namespace KSail.Commands.List.Handlers;

sealed class KSailListCommandHandler
{
  static readonly K3dProvisioner provisioner = new();
  internal static async Task HandleAsync() => await provisioner.ListAsync();
}
