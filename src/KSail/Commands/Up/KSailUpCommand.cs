using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Up.Binders;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Enums;
using KSail.Options;
using KSail.Services.Provisioners.ContainerEngine;
using KSail.Services.Provisioners.GitOps;
using KSail.Services.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Up;

sealed class KSailUpCommand : Command
{
  readonly ContainerEngineProvisionerBinder _containerEngineProvisionerBinder = new(ContainerEngineType.Docker);
  readonly KubernetesDistributionProvisionerBinder _kubernetesDistributionProvisionerBinder = new(KubernetesDistributionType.K3d);
  readonly GitOpsProvisionerBinder _gitOpsProvisionerBinder = new(GitOpsType.Flux);
  readonly NameArgument _nameArgument = new() { Arity = ArgumentArity.ExactlyOne };
  readonly ConfigOption _configOption = new() { IsRequired = true };
  readonly ManifestsOption _manifestsOption = new();
  readonly KustomizationsOption _kustomizationsOption = new();
  readonly TimeoutOption _timeoutOption = new();
  readonly NoSOPSOption _noSOPSOption = new();
  internal KSailUpCommand() : base("up", "Provision a K8s cluster")
  {
    AddArgument(_nameArgument);
    AddOption(_configOption);
    AddOption(_manifestsOption);
    AddOption(_kustomizationsOption);
    AddOption(_timeoutOption);
    AddOption(_noSOPSOption);

    AddValidator(result =>
    {
      string? name = result.GetValueForArgument(_nameArgument);
      if (string.IsNullOrEmpty(name))
      {
        result.ErrorMessage = "Required argument 'Name' missing for command: 'up'.";
        return;
      }
      string? configPath = $"{name}-{result.GetValueForOption(_configOption)}";
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
    this.SetHandler(async (containerEngineProvisioner, kubernetesDistributionProvisioner, gitOpsProvisioner, argumentsAndOptions) =>
    {
      argumentsAndOptions.Config = $"{argumentsAndOptions.Name}-{argumentsAndOptions.Config}";
      var handler = new KSailUpCommandHandler(containerEngineProvisioner, kubernetesDistributionProvisioner, gitOpsProvisioner);
      await handler.HandleAsync(
        argumentsAndOptions.Name,
        argumentsAndOptions.Config,
        argumentsAndOptions.Manifests,
        argumentsAndOptions.Kustomizations,
        argumentsAndOptions.Timeout,
        argumentsAndOptions.NoSOPS
      );
    }, _containerEngineProvisionerBinder, _kubernetesDistributionProvisionerBinder, _gitOpsProvisionerBinder, new KSailUpArgumentsAndOptionsBinder(_nameArgument, _configOption, _manifestsOption, _kustomizationsOption, _timeoutOption, _noSOPSOption));
  }
}
