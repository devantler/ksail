using KSail.Provisioners.Cluster;

namespace KSail.Commands.List.Handlers;

sealed class KSailListCommandHandler
{
  static readonly K3dProvisioner _provisioner = new();
  internal static async Task Handle() => await _provisioner.ListAsync();
}
