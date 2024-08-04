using KSail.CLIWrappers;

namespace KSail.Commands.Debug.Handlers;

class KSailDebugCommandHandler()
{
  internal static async Task<int> HandleAsync(string kubeconfig, string? context, CancellationToken token) => await K9sCLIWrapper.DebugClusterAsync(kubeconfig, context, token).ConfigureAwait(false) == 0 ? 0 : 1;
}
