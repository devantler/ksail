using KSail.CLIWrappers;

namespace KSail.Commands.Stop.Handlers;

static class KSailStopCommandHandler
{
  internal static Task<int> HandleAsync(string clusterName, CancellationToken token) => K3dCLIWrapper.StopClusterAsync(clusterName, token);
}
