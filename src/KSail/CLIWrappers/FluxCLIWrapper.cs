using CliWrap;
using IdentityModel;
using System.Runtime.InteropServices;

namespace KSail.CLIWrappers;

class FluxCLIWrapper()
{
  static Command Flux
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "flux_darwin-amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "flux_darwin-arm64",
        (PlatformID.Unix, Architecture.X64, false) => "flux_linux-amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "flux_linux-arm64",
        _ => throw new PlatformNotSupportedException()
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }

  internal static async Task CheckPrerequisitesAsync(string context)
  {
    var cmd = Flux.WithArguments($"check --pre --context {context}");
    _ = await CLIRunner.RunAsync(cmd);
  }

  internal static async Task InstallAsync(string context)
  {
    var cmd = Flux.WithArguments($"install --context {context}");
    _ = await CLIRunner.RunAsync(cmd);
  }

  internal static async Task CreateSourceOCIAsync(string context, string sourceUrl)
  {
    var cmd = Flux.WithArguments(
      [
        "create",
        "source",
        "oci",
        "flux-system",
        $"--url={sourceUrl}",
        "--insecure",
        "--tag=latest",
        $"--context={context}"
      ]
    );
    _ = await CLIRunner.RunAsync(cmd);
  }
  internal static async Task CreateKustomizationAsync(string context, string fluxKustomizationPath)
  {
    var cmd = Flux.WithArguments(
      [
        "create",
        "kustomization",
        "flux-system",
        "--source=OCIRepository/flux-system",
        $"--path={fluxKustomizationPath}",
        $"--context={context}"
      ]
    );
    _ = await CLIRunner.RunAsync(cmd);
  }
  internal static async Task UninstallAsync(string context)
  {
    var cmd = Flux.WithArguments($"uninstall --context {context}");
    _ = await CLIRunner.RunAsync(cmd);
  }

  internal static async Task PushManifestsAsync(string context, string ociUrl, string manifestsPath)
  {
    long currentTimeEpoch = DateTime.Now.ToEpochTime();
    var pushCmd = Flux.WithArguments(
      [
        "push",
        "artifact",
        $"{ociUrl}:{currentTimeEpoch}",
        $"--path={manifestsPath}",
        $"--source={ociUrl}",
        $"--revision={currentTimeEpoch}",
        $"--context={context}"
      ]
    );
    var tagCmd = Flux.WithArguments(
      [
        "tag",
        "artifact",
        $"{ociUrl}:{currentTimeEpoch}",
        "--tag=latest"
      ]
    );
    _ = await CLIRunner.RunAsync(pushCmd);
    _ = await CLIRunner.RunAsync(tagCmd);
  }

  internal static async Task ReconcileAsync(string name)
  {
    var cmd = Flux.WithArguments($"reconcile source oci flux-system --context {name}");
    _ = await CLIRunner.RunAsync(cmd);
  }
}
