using System.CommandLine;
using Devantler.ContainerEngineProvisioner.Docker;
using Devantler.KubernetesProvisioner.Cluster.K3d;
using Devantler.KubernetesProvisioner.GitOps.Flux;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Options;

namespace KSail.Commands.Up;

sealed class KSailUpCommand : Command
{
  readonly NameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ConfigOption _configOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ManifestsOption _manifestsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly KustomizationsOption _kustomizationsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly TimeoutOption _timeoutOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly SOPSOption _sopsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly SkipLintingOption _skipLintingOption = new() { Arity = ArgumentArity.ZeroOrOne };
  internal KSailUpCommand() : base("up", "Provision a cluster")
  {
    AddOption(_nameOption);
    AddOption(_configOption);
    AddOption(_manifestsOption);
    AddOption(_kustomizationsOption);
    AddOption(_timeoutOption);
    AddOption(_sopsOption);
    AddOption(_skipLintingOption);

    AddValidator(result =>
    {
      string? name = result.GetValueForOption(_nameOption);
      if (string.IsNullOrEmpty(name))
      {
        result.ErrorMessage = "Required argument 'ClusterName' missing for command: 'up'.";
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
    this.SetHandler(async (context) =>
    {
      var containerEngineProvisioner = new DockerProvisioner();
      var kubernetesDistributionProvisioner = new K3dProvisioner();
      var gitOpsProvisioner = new FluxProvisioner();

      string clusterName = context.ParseResult.GetValueForOption(_nameOption);
      string config = context.ParseResult.GetValueForOption(_configOption);
      string manifests = context.ParseResult.GetValueForOption(_manifestsOption)!;
      string kustomizations = context.ParseResult.GetValueForOption(_kustomizationsOption)!;
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
