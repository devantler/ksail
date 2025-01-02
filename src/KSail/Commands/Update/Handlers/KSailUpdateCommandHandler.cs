using Devantler.KubernetesProvisioner.GitOps.Flux;
using KSail.Commands.Lint.Handlers;
using KSail.Models;
using KSail.Models.Project;

namespace KSail.Commands.Update.Handlers;

class KSailUpdateCommandHandler
{
  readonly FluxProvisioner _deploymentTool;
  readonly KSailCluster _config;
  readonly KSailLintCommandHandler _ksailLintCommandHandler = new();

  internal KSailUpdateCommandHandler(KSailCluster config)
  {
    string context = $"{config.Spec.Project.Distribution.ToString().ToLower(System.Globalization.CultureInfo.CurrentCulture)}-{config.Metadata.Name}";
    _deploymentTool = config.Spec.Project.DeploymentTool switch
    {
      KSailDeploymentTool.Flux => new FluxProvisioner(context),
      _ => throw new NotSupportedException($"The deployment tool '{config.Spec.Project.DeploymentTool}' is not supported.")
    };
    _config = config;
  }

  internal async Task<bool> HandleAsync(CancellationToken cancellationToken = default)
  {
    if (!await Lint(_config, cancellationToken).ConfigureAwait(false))
    {
      return false;
    }
    switch (_config.Spec.Project.DeploymentTool)
    {
      case KSailDeploymentTool.Flux:
        var ksailRegistryUri = _config.Spec.FluxDeploymentToolOptions.Source.Url;
        Console.WriteLine($"📥 Pushing manifests to '{ksailRegistryUri}'");
        // TODO: Make some form of abstraction around GitOps tools, so it is easier to support apply-based tools like kubectl
        await _deploymentTool.PushManifestsAsync(ksailRegistryUri, "k8s", cancellationToken: cancellationToken).ConfigureAwait(false);
        Console.WriteLine();
        if (_config.Spec.CLIOptions.UpdateOptions.Reconcile)
        {
          Console.WriteLine("🔄 Reconciling changes");
          await _deploymentTool.ReconcileAsync(_config.Spec.KustomizeTemplateOptions.Kustomizations.Reverse().ToArray(), _config.Spec.Connection.Timeout, cancellationToken).ConfigureAwait(false);
        }
        Console.WriteLine();
        break;
      default:
        throw new NotSupportedException($"The deployment tool '{_config.Spec.Project.DeploymentTool}' is not supported.");
    }


    return true;
  }

  async Task<bool> Lint(KSailCluster config, CancellationToken cancellationToken = default)
  {
    if (config.Spec.CLIOptions.UpdateOptions.Lint)
    {
      Console.WriteLine("🔍 Linting manifests");
      bool success = await _ksailLintCommandHandler.HandleAsync(config, cancellationToken).ConfigureAwait(false);
      Console.WriteLine();
      return success;
    }
    return true;
  }
}
