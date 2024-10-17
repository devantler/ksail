using Devantler.K3dCLI;
using Devantler.K9sCLI;
using Devantler.KubernetesGenerator.KSail.Models;

namespace KSail.Commands.Debug.Handlers;

class KSailDebugCommandHandler
{
  readonly KSailCluster _config;

  internal KSailDebugCommandHandler(KSailCluster config) => _config = config;

  internal async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    try
    {
      await K9s.RunAsync((Editor)_config?.Spec?.DebugOptions?.Editor!, _config.Spec.Kubeconfig, _config.Spec.Context, cancellationToken).ConfigureAwait(false);
      return 0;
    }
    catch (K3dException ex)
    {
      Console.WriteLine($"🚨 K9s failed to start: {ex.Message}");
      return 1;
    }
  }
}
