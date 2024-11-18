using Devantler.K9sCLI;
using KSail.Models;

namespace KSail.Commands.Debug.Handlers;

class KSailDebugCommandHandler
{
  readonly KSailCluster _config;

  internal KSailDebugCommandHandler(KSailCluster config) => _config = config;

  internal async Task<bool> HandleAsync(CancellationToken cancellationToken = default)
  {
    await K9s.RunAsync(_config.Spec.CLI.DebugOptions.Editor, _config.Spec.Connection.Kubeconfig, _config.Spec.Connection.Context, cancellationToken).ConfigureAwait(false);
    return true;
  }
}
