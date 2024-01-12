using System.CommandLine;
using KSail.Commands.Check.Handlers;
using KSail.Commands.Lint.Handlers;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Commands.Up.Validators;
using KSail.Models;
using KSail.Options;
using YamlDotNet.Serialization;

namespace KSail.Commands.Up;

sealed class KSailUpGitOpsCommand : Command
{
  readonly ManifestsOption manifestsOption = new() { IsRequired = true };
  readonly FluxKustomizationPathOption fluxKustomizationPathOption = new();
  readonly TimeoutOption timeoutOption = new();
  readonly SOPSOption sopsOption = new() { IsRequired = true };
  static readonly Deserializer yamlDeserializer = new();

  internal KSailUpGitOpsCommand(
    NameOption nameOption,
    PullThroughRegistriesOption pullThroughRegistriesOption,
    ConfigOption configOption
  ) : base("gitops", "Create a GitOps-enabled K8s cluster")
  {
    AddOption(manifestsOption);
    AddOption(fluxKustomizationPathOption);
    AddOption(timeoutOption);
    AddOption(sopsOption);

    AddValidator(
      async commandResult => await KSailUpGitOpsValidator.ValidateAsync(
        commandResult, nameOption,
        configOption, manifestsOption,
        fluxKustomizationPathOption
      )
    );
    this.SetHandler(async (name, configPath, manifestsPath, fluxKustomizationPath, timeout, pullThroughRegistries, sops) =>
    {
      var config = string.IsNullOrEmpty(configPath) ? null : yamlDeserializer.Deserialize<K3dConfig>(File.ReadAllText(configPath));
      name = config?.Metadata.Name ?? name;
      fluxKustomizationPath = string.IsNullOrEmpty(fluxKustomizationPath) ? $"clusters/{name}/flux" : fluxKustomizationPath;
      await KSailLintCommandHandler.HandleAsync(name, manifestsPath);
      await KSailUpCommandHandler.HandleAsync(name, configPath, pullThroughRegistries);
      await KSailUpGitOpsCommandHandler.HandleAsync(name, manifestsPath, fluxKustomizationPath, sops);
      await KSailCheckCommandHandler.HandleAsync(name, new CancellationToken());
    }, nameOption, configOption, manifestsOption, fluxKustomizationPathOption, pullThroughRegistriesOption, sopsOption);
  }
}
