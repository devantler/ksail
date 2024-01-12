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
  readonly ManifestsPathOption manifestsPathOption = new() { IsRequired = true };
  readonly FluxKustomizationPathOption fluxKustomizationPathOption = new();
  readonly TimeoutOption timeoutOption = new();
  readonly SOPSOption sopsOption = new() { IsRequired = true };
  static readonly Deserializer yamlDeserializer = new();

  internal KSailUpGitOpsCommand(
    NameOption nameOption,
    PullThroughRegistriesOption pullThroughRegistriesOption,
    ConfigPathOption configPathOption
  ) : base("gitops", "Create a K8s cluster with GitOps")
  {
    AddOption(manifestsPathOption);
    AddOption(fluxKustomizationPathOption);
    AddOption(timeoutOption);
    AddOption(sopsOption);

    AddValidator(
      async commandResult => await KSailUpGitOpsValidator.ValidateAsync(
        commandResult, nameOption,
        configPathOption, manifestsPathOption,
        fluxKustomizationPathOption
      )
    );
    this.SetHandler(async (name, configPath, manifestsPath, fluxKustomizationPath, timeout, pullThroughRegistries, sops) =>
    {
      var config = string.IsNullOrEmpty(configPath) ? null : yamlDeserializer.Deserialize<K3dConfig>(File.ReadAllText(configPath));
      name = config?.Metadata.Name ?? name;
      fluxKustomizationPath = string.IsNullOrEmpty(fluxKustomizationPath) ? $"clusters/{name}/flux" : fluxKustomizationPath;
      await KSailLintCommandHandler.HandleAsync(name, manifestsPath);
      await KSailUpCommandHandler.HandleAsync(name, pullThroughRegistries, configPath);
      await KSailUpGitOpsCommandHandler.HandleAsync(name, manifestsPath, fluxKustomizationPath, sops);
      await KSailCheckCommandHandler.HandleAsync(name, timeout, new CancellationToken());
    }, nameOption, configPathOption, manifestsPathOption, fluxKustomizationPathOption, timeoutOption, pullThroughRegistriesOption, sopsOption);
  }
}
