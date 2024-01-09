using System.CommandLine;
using KSail.Commands.Lint.Handlers;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Models.K3d;
using KSail.Options;
using YamlDotNet.Serialization;

namespace KSail.Commands.Up;

sealed class KSailUpK3dFluxCommand : Command
{
  readonly ManifestsPathOption _manifestsPathOption = new() { IsRequired = true };
  readonly FluxKustomizationPathOption _fluxKustomizationPathOption = new() { IsRequired = true };
  readonly SOPSOption _sopsOption = new() { IsRequired = true };
  static readonly Deserializer _yamlDeserializer = new();

  internal KSailUpK3dFluxCommand(NameOption nameOption, PullThroughRegistriesOption pullThroughRegistriesOption, ConfigPathOption configPathOption) : base("flux", "create a K3d cluster with Flux GitOps")
  {
    AddOption(_manifestsPathOption);
    AddOption(_fluxKustomizationPathOption);
    AddOption(_sopsOption);
    this.SetHandler(async (name, pullThroughRegistries, configPath, manifestsPath, _fluxKustomizationPath, sops) =>
    {
      if (!string.IsNullOrEmpty(configPath))
      {
        var deserializedConfig = _yamlDeserializer.Deserialize<K3dConfig>(File.ReadAllText(configPath));
        name = deserializedConfig.Metadata.Name;
      }
      await KSailLintCommandHandler.HandleAsync(name, manifestsPath);
      await KSailUpK3dCommandHandler.HandleAsync(name, pullThroughRegistries, configPath);
      await KSailUpK3dFluxCommandHandler.HandleAsync(name, manifestsPath, _fluxKustomizationPath, sops);
    }, nameOption, pullThroughRegistriesOption, configPathOption, _manifestsPathOption, _fluxKustomizationPathOption, _sopsOption);
  }
}
