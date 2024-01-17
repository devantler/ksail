using KSail.Provisioners;

namespace KSail.Commands.List.Handlers;

internal sealed class KSailListCommandHandler
{
  internal static async Task HandleAsync() => await K3dProvisioner.ListAsync();
}
