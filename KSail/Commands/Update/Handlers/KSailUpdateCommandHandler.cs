using Devantler.KubernetesProvisioner.GitOps.Flux;
using KSail.Commands.Lint.Handlers;
using KSail.Models;

namespace KSail.Commands.Update.Handlers;

class KSailUpdateCommandHandler
{
  readonly FluxProvisioner _gitOpsProvisioner;
  readonly KSailCluster _config;

  internal KSailUpdateCommandHandler(KSailCluster config)
  {
    string context = $"{config.Spec.Distribution.ToString().ToLower(System.Globalization.CultureInfo.CurrentCulture)}-{config.Metadata.Name}";
    _gitOpsProvisioner = config.Spec.GitOpsTool switch
    {
      KSailGitOpsTool.Flux => new FluxProvisioner(context),
      _ => throw new NotSupportedException($"The GitOps tool '{config.Spec.GitOpsTool}' is not supported.")
    };
    _config = config;
  }

  internal async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    if (_config.Spec.UpdateOptions.Lint)
    {
      _ = await KSailLintCommandHandler.HandleAsync(_config, cancellationToken).ConfigureAwait(false);
    };

    var ksailRegistryUri = new Uri($"oci://localhost:{_config.Spec.Registries.First().HostPort}/{_config.Metadata.Name}");

    Console.WriteLine($"📥 Pushing manifests to {_config.Spec.Registries.First().Name} on '{ksailRegistryUri}'");
    await _gitOpsProvisioner.PushManifestsAsync(ksailRegistryUri, _config.Spec.ManifestsDirectory, cancellationToken: cancellationToken).ConfigureAwait(false);

    if (_config.Spec.UpdateOptions.Reconcile)
    {
      Console.WriteLine("");
      Console.WriteLine("🔄 Reconciling changes");
      await _gitOpsProvisioner.ReconcileAsync(cancellationToken).ConfigureAwait(false);
    }
    Console.WriteLine("");
    return 0;
  }
}
