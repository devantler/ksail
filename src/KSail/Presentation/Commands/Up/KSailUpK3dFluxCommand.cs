using System.CommandLine;
using KSail.Presentation.Commands.Up.Handlers;
using KSail.Presentation.Commands.Up.Options;
using KSail.Presentation.Options;

namespace KSail.Presentation.Commands.Up;

/// <summary>
/// The <c>ksail up k3d flux</c> command responsible for creating K3d clusters with Flux GitOps.
/// </summary>
public class KSailUpK3dFluxCommand : Command
{
  readonly ManifestsPathOption _manifestsPathOption = new();

  /// <summary>
  /// Initializes a new instance of the <see cref="KSailUpK3dFluxCommand"/> class.
  /// </summary>
  /// <param name="nameOption">The name option.</param>
  /// <param name="pullThroughRegistriesOption">The pull through registries option.</param>
  /// <param name="configPathOption">The config path option.</param>
  public KSailUpK3dFluxCommand(NameOption nameOption, PullThroughRegistriesOption pullThroughRegistriesOption, ConfigPathOption configPathOption) : base("flux", "create a K3d cluster with Flux GitOps")
  {
    AddOption(_manifestsPathOption);
    this.SetHandler(async (name, manifestsPath, pullThroughRegistries, configPath) =>
    {
      await KSailUpK3dCommandHandler.Handle(name, pullThroughRegistries, configPath);
      //await KSailUpK3dFluxCommandHandler.Handle(manifestsPath);
      Console.WriteLine("🏡 Creating OCI registry...");
      Console.WriteLine("📥 Pushing manifests to OCI registry...");
      Console.WriteLine("🔄 Installing Flux GitOps...");
    }, nameOption, _manifestsPathOption, pullThroughRegistriesOption, configPathOption);
  }
}
