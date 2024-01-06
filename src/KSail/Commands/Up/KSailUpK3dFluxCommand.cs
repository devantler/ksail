using System.CommandLine;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Options;
using KSail.Presentation.Commands.Up.Options;

namespace KSail.Commands.Up;

/// <summary>
/// The <c>ksail up k3d flux</c> command responsible for creating K3d clusters with Flux GitOps.
/// </summary>
public class KSailUpK3dFluxCommand : Command
{
  readonly ManifestsPathOption _manifestsPathOption = new();
  readonly SOPSOption _sopsOption = new();

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailUpK3dFluxCommand"/> class.
  /// </summary>
  /// <param name="nameOption">The name option.</param>
  /// <param name="pullThroughRegistriesOption">The pull through registries option.</param>
  /// <param name="configPathOption">The config path option.</param>
  public KSailUpK3dFluxCommand(NameOption nameOption, PullThroughRegistriesOption pullThroughRegistriesOption, ConfigPathOption configPathOption) : base("flux", "create a K3d cluster with Flux GitOps")
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
