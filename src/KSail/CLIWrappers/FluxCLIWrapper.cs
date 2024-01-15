using CliWrap;
using IdentityModel;
using System.CommandLine;
using System.Runtime.InteropServices;

namespace KSail.CLIWrappers;

class FluxCLIWrapper(IConsole console)
{
  readonly CLIRunner cliRunner = new(console);
  static CliWrap.Command Flux
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

  internal async Task CheckPrerequisitesAsync()
  {
    var cmd = Flux.WithArguments("check --pre");
    _ = await cliRunner.RunAsync(cmd);
  }

  internal async Task InstallAsync()
  {
    var cmd = Flux.WithArguments("install");
    _ = await cliRunner.RunAsync(cmd);
  }

  internal async Task CreateSourceOCIAsync(string sourceUrl)
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
    _ = await cliRunner.RunAsync(cmd);
  }
  internal async Task CreateKustomizationAsync(string fluxKustomizationPathOption)
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
    _ = await cliRunner.RunAsync(cmd);
  }
  internal async Task UninstallAsync()
  {
    var cmd = Flux.WithArguments("uninstall");
    _ = await cliRunner.RunAsync(cmd);
  }

  internal async Task PushManifestsAsync(string ociUrl, string manifestsPath)
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
    _ = await cliRunner.RunAsync(pushCmd);
    _ = await cliRunner.RunAsync(tagCmd);
  }
}
