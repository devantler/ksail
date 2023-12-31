using CliWrap;
using IdentityModel;
using System.Runtime.InteropServices;

namespace KSail.CLIWrappers;

static class FluxCLIWrapper
{
  static Command Flux
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "flux_darwin_amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "flux_darwin_arm64",
        (PlatformID.Unix, Architecture.X64, false) => "flux_linux_amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "flux_linux_arm64",
        (PlatformID.Unix, Architecture.Arm, false) => "flux_linux_arm",
        _ => throw new PlatformNotSupportedException()
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }

  internal static async Task CheckPrerequisitesAsync()
  {
    var cmd = Flux.WithArguments("check --pre");
    _ = await CLIRunner.RunAsync(cmd);
  }

  internal static async Task InstallAsync()
  {
    var cmd = Flux.WithArguments("install");
    _ = await CLIRunner.RunAsync(cmd);
  }

  internal static async Task CreateSourceOCIAsync(string sourceUrl)
  {
    var cmd = Flux.WithArguments(
      [
        "create",
        "source",
        "oci",
        "flux-system",
        $"--url={sourceUrl}",
        "--insecure",
        "--tag=latest"
      ]
    );
    _ = await CLIRunner.RunAsync(cmd);
  }
  internal static async Task CreateKustomizationAsync(string fluxKustomizationPathOption)
  {
    var cmd = Flux.WithArguments(
      [
        "create",
        "kustomization",
        "flux-system",
        "--source=OCIRepository/flux-system",
        $"--path={fluxKustomizationPathOption}"
      ]
    );
    _ = await CLIRunner.RunAsync(cmd);
  }
  internal static async Task UninstallAsync()
  {
    var cmd = Flux.WithArguments("uninstall");
    _ = await CLIRunner.RunAsync(cmd);
  }

  internal static async Task PushManifestsAsync(string ociUrl, string manifestsPath)
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
}
