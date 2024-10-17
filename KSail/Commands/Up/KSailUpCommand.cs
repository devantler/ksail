using System.CommandLine;
using Devantler.ContainerEngineProvisioner.Docker;
using Devantler.KubernetesProvisioner.Cluster.K3d;
using Devantler.KubernetesProvisioner.GitOps.Flux;
using KSail.Commands.Init.Options;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Options;

namespace KSail.Commands.Up;

sealed class KSailUpCommand : Command
{
  readonly NameOption _clusterNameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ConfigOption _configOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ManifestsOption _manifestsOption = new();
  readonly KustomizationsOption _kustomizationsOption = new();
  readonly TimeoutOption _timeoutOption = new();
  readonly SOPSOption _sopsOption = new();
  readonly SkipLintingOption _skipLintingOption = new();
  internal KSailUpCommand() : base("up", "Provision a cluster")
  {
    AddOption(_clusterNameOption);
    AddOption(_configOption);
    AddOption(_manifestsOption);
    AddOption(_kustomizationsOption);
    AddOption(_timeoutOption);
    AddOption(_sopsOption);
    AddOption(_skipLintingOption);

    AddValidator(result =>
    {
      string? clusterName = result.GetValueForArgument(_clusterNameOption);
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
      var gitOpsProvisioner = new FluxProvisioner();

      string clusterName = context.ParseResult.GetValueForOption(_clusterNameOption);
      string? config = context.ParseResult.GetValueForOption(_configOption);
      string? manifests = context.ParseResult.GetValueForOption(_manifestsOption) ??
        throw new InvalidOperationException($"Required option '{_manifestsOption.Name}' missing for command: 'up'.");
      string? kustomizations = context.ParseResult.GetValueForOption(_kustomizationsOption) ??
        $"clusters/{clusterName}/flux-system";
      int timeout = context.ParseResult.GetValueForOption(_timeoutOption);
      bool noSOPS = context.ParseResult.GetValueForOption(_sopsOption);
      bool skipLintingOption = context.ParseResult.GetValueForOption(_skipLintingOption);

      config = $"{clusterName}-{config}";

      var cancellationToken = context.GetCancellationToken();
      var handler = new KSailUpCommandHandler(containerEngineProvisioner, kubernetesDistributionProvisioner, gitOpsProvisioner);
      try
      {
        context.ExitCode = await handler.HandleAsync(
          clusterName,
          config,
          manifests,
          kustomizations,
          timeout,
          noSOPS,
          skipLintingOption,
          cancellationToken
        ).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
