using System.CommandLine;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Options;
using KSail.Presentation.Commands.Up.Options;

namespace KSail.Commands.Up;

sealed class KSailUpK3dFluxCommand : Command
{
  readonly ManifestsPathOption _manifestsPathOption = new();
  readonly SOPSOption _sopsOption = new();

  internal KSailUpK3dFluxCommand(NameOption nameOption, PullThroughRegistriesOption pullThroughRegistriesOption, ConfigPathOption configPathOption) : base("flux", "create a K3d cluster with Flux GitOps")
  {
    AddOption(_manifestsPathOption);
    AddOption(_sopsOption);
    this.SetHandler(async (name, pullThroughRegistries, configPath, manifestsPath, sops) =>
    {
      await KSailUpK3dCommandHandler.Handle(name, pullThroughRegistries, configPath);
      await KSailUpK3dFluxCommandHandler.Handle(manifestsPath, sops);
    }, nameOption, pullThroughRegistriesOption, configPathOption, _manifestsPathOption, _sopsOption);
  }
}
