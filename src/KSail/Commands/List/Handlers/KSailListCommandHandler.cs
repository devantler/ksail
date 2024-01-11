using KSail.Provisioners;

namespace KSail.Commands.List.Handlers;

sealed class KSailListCommandHandler
{
  internal static async Task HandleAsync() => await K3dProvisioner.ListAsync();
}
