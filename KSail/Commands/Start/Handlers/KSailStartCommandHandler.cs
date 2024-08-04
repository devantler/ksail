using KSail.CLIWrappers;

namespace KSail.Commands.Start.Handlers;

static class KSailStartCommandHandler
{
  internal static Task<int> HandleAsync(string clusterName, CancellationToken token) => K3dCLIWrapper.StartClusterAsync(clusterName, token);
}
