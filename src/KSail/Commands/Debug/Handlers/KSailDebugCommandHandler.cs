using KSail.CLIWrappers;

namespace KSail.Commands.Debug.Handlers;

class KSailDebugCommandHandler()
{
  internal static async Task<int> HandleAsync(string? context, CancellationToken token) => await K9sCLIWrapper.DebugClusterAsync(context, token) == 0 ? 0 : 1;
}
