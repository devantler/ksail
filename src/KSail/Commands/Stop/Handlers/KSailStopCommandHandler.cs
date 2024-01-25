using KSail.CLIWrappers;

namespace KSail.Commands.Stop.Handlers;

static class KSailStopCommandHandler
{
  internal static Task HandleAsync(string name) => K3dCLIWrapper.StopClusterAsync(name);
}
