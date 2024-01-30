using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Options;
using KSail.Provisioners.ContainerEngine;
using KSail.Provisioners.ContainerOrchestrator;
using KSail.Provisioners.GitOps;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.Up;

sealed class KSailUpCommand : Command
{
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
    this.SetHandler(async (context) =>
    {
      var containerEngineProvisioner = new DockerProvisioner();
      var kubernetesDistributionProvisioner = new K3dProvisioner();
      var containerOrchestratorProvisioner = new KubernetesProvisioner();
      var gitOpsProvisioner = new FluxProvisioner();

      string clusterName = context.ParseResult.GetValueForArgument(_clusterNameArgument);
      string? config = context.ParseResult.GetValueForOption(_configOption);
      string? manifests = context.ParseResult.GetValueForOption(_manifestsOption) ??
        throw new InvalidOperationException($"Required option '{_manifestsOption.Name}' missing for command: 'up'.");
      string? kustomizations = context.ParseResult.GetValueForOption(_kustomizationsOption) ??
        $"clusters/{clusterName}/flux-system";
      int timeout = context.ParseResult.GetValueForOption(_timeoutOption);
      bool noSOPS = context.ParseResult.GetValueForOption(_noSOPSOption);

      config = $"{clusterName}-{config}";

      var token = context.GetCancellationToken();
      var handler = new KSailUpCommandHandler(containerEngineProvisioner, kubernetesDistributionProvisioner, containerOrchestratorProvisioner, gitOpsProvisioner);
      try
      {
        _ = await handler.HandleAsync(
          clusterName,
          config,
          manifests,
          kustomizations,
          timeout,
          noSOPS,
          token
        );
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
