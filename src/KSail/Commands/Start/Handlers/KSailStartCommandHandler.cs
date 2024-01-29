using KSail.CLIWrappers;

namespace KSail.Commands.Start.Handlers;

static class KSailStartCommandHandler
{
  internal static Task HandleAsync(string clusterName) => K3dCLIWrapper.StartClusterAsync(clusterName);
}
