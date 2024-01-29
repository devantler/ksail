using KSail.CLIWrappers;

namespace KSail.Commands.Stop.Handlers;

static class KSailStopCommandHandler
{
  internal static Task HandleAsync(string clusterName) => K3dCLIWrapper.StopClusterAsync(clusterName);
}
