using KSail.CLIWrappers;

namespace KSail.Commands.Start.Handlers;

static class KSailStartCommandHandler
{
  internal static async Task HandleAsync(string name) => await K3dCLIWrapper.StartClusterAsync(name);
}
