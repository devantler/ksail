using System.CommandLine;
using KSail.Commands.Lint.Handlers;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Commands.Up.Validators;
using KSail.Models.K3d;
using KSail.Options;
using YamlDotNet.Serialization;

namespace KSail.Commands.Up;

sealed class KSailUpK3dFluxCommand : Command
{
  readonly ManifestsPathOption manifestsPathOption = new() { IsRequired = true };
  readonly FluxKustomizationPathOption fluxKustomizationPathOption = new();
  readonly SOPSOption sopsOption = new() { IsRequired = true };
  static readonly Deserializer yamlDeserializer = new();

  internal KSailUpK3dFluxCommand(
    NameOption nameOption,
    PullThroughRegistriesOption pullThroughRegistriesOption,
    ConfigPathOption configPathOption
  ) : base("flux", "create a K3d cluster with Flux GitOps")
  {
    AddOption(manifestsPathOption);
    AddOption(fluxKustomizationPathOption);
    AddOption(sopsOption);

    AddValidator(
      async commandResult => await KSailUpK3dFluxValidator.ValidateAsync(
        commandResult, nameOption,
        configPathOption, manifestsPathOption,
        fluxKustomizationPathOption
      )
    );
    this.SetHandler(async (name, configPath, manifestsPath, _fluxKustomizationPath, pullThroughRegistries, sops) =>
    {
      var config = string.IsNullOrEmpty(configPath) ? null : yamlDeserializer.Deserialize<K3dConfig>(File.ReadAllText(configPath));
      name = config?.Metadata.Name ?? name;
      _fluxKustomizationPath = string.IsNullOrEmpty(_fluxKustomizationPath) ? $"clusters/{name}/flux" : _fluxKustomizationPath;
      await KSailLintCommandHandler.HandleAsync(name, manifestsPath);
      await KSailUpK3dCommandHandler.HandleAsync(name, pullThroughRegistries, configPath);
      await KSailUpK3dFluxCommandHandler.HandleAsync(name, manifestsPath, _fluxKustomizationPath, sops);
    }, nameOption, configPathOption, manifestsPathOption, fluxKustomizationPathOption, pullThroughRegistriesOption, sopsOption);
  }
}
