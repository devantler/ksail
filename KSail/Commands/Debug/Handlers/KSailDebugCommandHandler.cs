using Devantler.K9sCLI;
using KSail.Models;

namespace KSail.Commands.Debug.Handlers;

class KSailDebugCommandHandler
{
  readonly KSailCluster _config;

  internal KSailDebugCommandHandler(KSailCluster config) => _config = config;

  internal async Task<bool> HandleAsync(CancellationToken cancellationToken = default)
  {
    await K9s.RunAsync(_config.Spec.DebugOptions.Editor, _config.Spec.Kubeconfig, _config.Spec.Context, cancellationToken).ConfigureAwait(false);
    return true;
  }
}
