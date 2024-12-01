using Devantler.KubernetesProvisioner.GitOps.Flux;
using KSail.Commands.Lint.Handlers;
using KSail.Models;
using KSail.Models.Project;

namespace KSail.Commands.Update.Handlers;

class KSailUpdateCommandHandler
{
  readonly FluxProvisioner _gitOpsProvisioner;
  readonly KSailCluster _config;
  readonly KSailLintCommandHandler _ksailLintCommandHandler = new();

  internal KSailUpdateCommandHandler(KSailCluster config)
  {
    string context = $"{config.Spec.Project.Distribution.ToString().ToLower(System.Globalization.CultureInfo.CurrentCulture)}-{config.Metadata.Name}";
    _gitOpsProvisioner = config.Spec.Project.GitOpsTool switch
    {
      KSailGitOpsTool.Flux => new FluxProvisioner(context),
      _ => throw new NotSupportedException($"The GitOps tool '{config.Spec.Project.GitOpsTool}' is not supported.")
    };
    _config = config;
  }

  internal async Task<bool> HandleAsync(CancellationToken cancellationToken = default)
  {
    if (!await Lint(_config, cancellationToken).ConfigureAwait(false))
    {
      return false;
    }

    var ksailRegistryUri = new Uri($"oci://localhost:{_config.Spec.Registries.First().HostPort}/{_config.Metadata.Name}");
    Console.WriteLine($"📥 Pushing manifests to {_config.Spec.Registries.First().Name} on '{ksailRegistryUri}'");
    await _gitOpsProvisioner.PushManifestsAsync(ksailRegistryUri, _config.Spec.Project.ManifestsDirectory, cancellationToken: cancellationToken).ConfigureAwait(false);
    Console.WriteLine();

    if (_config.Spec.CLI.UpdateOptions.Reconcile)
    {
      Console.WriteLine("🔄 Reconciling changes");
      await _gitOpsProvisioner.ReconcileAsync(_config.Spec.Project.KustomizeFlows.ToArray(), _config.Spec.Connection.Timeout, cancellationToken).ConfigureAwait(false);
    }
    Console.WriteLine();
    return true;
  }

  async Task<bool> Lint(KSailCluster config, CancellationToken cancellationToken = default)
  {
    if (config.Spec.CLI.UpdateOptions.Lint)
    {
      Console.WriteLine("🔍 Linting manifests");
      bool success = await _ksailLintCommandHandler.HandleAsync(config, cancellationToken).ConfigureAwait(false);
      Console.WriteLine();
      return success;
    }
    return true;
  }
}
