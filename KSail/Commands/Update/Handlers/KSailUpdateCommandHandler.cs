using Devantler.KubernetesProvisioner.GitOps.Flux;
using KSail.Commands.Lint.Handlers;

namespace KSail.Commands.Update.Handlers;

class KSailUpdateCommandHandler
{
  readonly FluxProvisioner _gitOpsProvisioner;

  internal KSailUpdateCommandHandler(KSailCluster config)
  {
    string context = $"{config.Spec?.Distribution?.ToString()?.ToLower(System.Globalization.CultureInfo.CurrentCulture)}-{config.Metadata.Name}";
    _gitOpsProvisioner = config.Spec?.GitOpsTool switch
    {
      KSailGitOpsTool.Flux => new FluxProvisioner(context),
      _ => throw new NotSupportedException($"The GitOps tool '{config.Spec?.GitOpsTool}' is not supported.")
    };
  }

  internal async Task HandleAsync(KSailCluster config, CancellationToken cancellationToken)
  {
    if (config.Spec?.UpdateOptions?.Lint == true)
    {
      await KSailLintCommandHandler.HandleAsync(config, cancellationToken).ConfigureAwait(false);
    };

    var ksailRegistryUri = new Uri($"oci://localhost:{config.Spec?.Registries?.First().HostPort}/{config.Metadata.Name}");

    Console.WriteLine($"📥 Pushing manifests to {config.Spec?.Registries?.First().Name} on '{ksailRegistryUri}'");
    await _gitOpsProvisioner.PushManifestsAsync(ksailRegistryUri, config.Spec?.ManifestsDirectory!, cancellationToken: cancellationToken).ConfigureAwait(false);

    if (config.Spec?.UpdateOptions?.Reconcile == true)
    {

      Console.WriteLine("");
      Console.WriteLine("🔄 Reconciling Flux");
      await _gitOpsProvisioner.ReconcileAsync(cancellationToken).ConfigureAwait(false);
    }
    Console.WriteLine("");
  }
}
