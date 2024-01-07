using System.CommandLine;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Options;

namespace KSail.Commands.Up;

sealed class KSailUpK3dFluxCommand : Command
{
  readonly ManifestsPathOption _manifestsPathOption = new();
  readonly FluxKustomizationPathOption _fluxKustomizationPathOption = new();
  readonly SOPSOption _sopsOption = new();

  internal KSailUpK3dFluxCommand(NameOption nameOption, PullThroughRegistriesOption pullThroughRegistriesOption, ConfigPathOption configPathOption) : base("flux", "create a K3d cluster with Flux GitOps")
  {
    AddOption(_manifestsPathOption);
    AddOption(_fluxKustomizationPathOption);
    AddOption(_sopsOption);
    this.SetHandler(async (name, pullThroughRegistries, configPath, manifestsPath, _fluxKustomizationPath, sops) =>
    {
      bool shouldPrompt = string.IsNullOrEmpty(name) && string.IsNullOrEmpty(configPath);
      await KSailUpK3dCommandHandler.Handle(shouldPrompt, name, pullThroughRegistries, configPath);
      await KSailUpK3dFluxCommandHandler.Handle(shouldPrompt, name, manifestsPath, _fluxKustomizationPath, sops);
    }, nameOption, pullThroughRegistriesOption, configPathOption, _manifestsPathOption, _fluxKustomizationPathOption, _sopsOption);
  }
}
