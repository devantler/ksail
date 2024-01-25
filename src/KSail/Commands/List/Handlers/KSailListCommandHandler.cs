using KSail.Provisioners;

namespace KSail.Commands.List.Handlers;

sealed class KSailListCommandHandler
{
  internal static Task HandleAsync() => K3dProvisioner.ListAsync();
}
