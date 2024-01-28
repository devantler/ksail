using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Up.Binders;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Enums;
using KSail.Options;
using KSail.Services.Provisioners.ContainerEngine;
using KSail.Services.Provisioners.ContainerOrchestrator;
using KSail.Services.Provisioners.GitOps;
using KSail.Services.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Up;

sealed class KSailUpCommand : Command
{
  readonly ContainerEngineProvisionerBinder _containerEngineProvisionerBinder = new(ContainerEngineType.Docker);
  readonly KubernetesDistributionProvisionerBinder _kubernetesDistributionProvisionerBinder = new(KubernetesDistributionType.K3d);
  readonly ContainerOrchestratorProvisionerBinder _containerOrchestratorProvisionerBinder = new(ContainerOrchestratorType.Kubernetes);
  readonly GitOpsProvisionerBinder _gitOpsProvisionerBinder = new(GitOpsType.Flux);
  readonly ClusterNameArgument _clusterNameArgument = new() { Arity = ArgumentArity.ExactlyOne };
  readonly ConfigOption _configOption = new() { IsRequired = true };
  readonly ManifestsOption _manifestsOption = new();
  readonly KustomizationsOption _kustomizationsOption = new();
  readonly TimeoutOption _timeoutOption = new();
  readonly NoSOPSOption _noSOPSOption = new();
  internal KSailUpCommand() : base("up", "Provision a K8s cluster")
  {
    AddArgument(_clusterNameArgument);
    AddOption(_configOption);
    AddOption(_manifestsOption);
    AddOption(_kustomizationsOption);
    AddOption(_timeoutOption);
    AddOption(_noSOPSOption);

    AddValidator(result =>
    {
      string? clusterName = result.GetValueForArgument(_clusterNameArgument);
      if (string.IsNullOrEmpty(clusterName))
      {
        result.ErrorMessage = "Required argument 'ClusterName' missing for command: 'up'.";
        return;
      }
      string? configPath = $"{clusterName}-{result.GetValueForOption(_configOption)}";
      string? manifestsPath = result.GetValueForOption(_manifestsOption);
      if (string.IsNullOrEmpty(configPath) || !File.Exists(configPath))
      {
        result.ErrorMessage = $"Config file '{configPath}' does not exist";
      }
      else if (string.IsNullOrEmpty(manifestsPath) || !Directory.Exists(manifestsPath))
      {
        result.ErrorMessage = $"Manifests directory '{manifestsPath}' does not exist";
      }
    });
    this.SetHandler(async (containerEngineProvisioner, kubernetesDistributionProvisioner, containerOrchestratorProvisioner, gitOpsProvisioner, argumentsAndOptions) =>
    {
      argumentsAndOptions.Config = $"{argumentsAndOptions.ClusterName}-{argumentsAndOptions.Config}";
      var handler = new KSailUpCommandHandler(containerEngineProvisioner, kubernetesDistributionProvisioner, containerOrchestratorProvisioner, gitOpsProvisioner);
      await handler.HandleAsync(
        argumentsAndOptions.ClusterName,
        argumentsAndOptions.Config,
        argumentsAndOptions.Manifests,
        argumentsAndOptions.Kustomizations,
        argumentsAndOptions.Timeout,
        argumentsAndOptions.NoSOPS
      );
    }, _containerEngineProvisionerBinder, _kubernetesDistributionProvisionerBinder, _containerOrchestratorProvisionerBinder, _gitOpsProvisionerBinder, new KSailUpArgumentsAndOptionsBinder(_clusterNameArgument, _configOption, _manifestsOption, _kustomizationsOption, _timeoutOption, _noSOPSOption));
  }
}
