using KSail.CLIWrappers;

namespace KSail.Commands.Stop.Handlers;

internal static class KSailStopCommandHandler
{
  internal static async Task HandleAsync(string name) => await K3dCLIWrapper.StopClusterAsync(name);
}
