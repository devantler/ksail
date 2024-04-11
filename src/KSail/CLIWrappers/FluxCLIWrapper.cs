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
        _ => throw new PlatformNotSupportedException($"🚨 Unsupported platform: {Environment.OSVersion.Platform} {RuntimeInformation.ProcessArchitecture}"),
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }

  internal static async Task<int> CheckPrerequisitesAsync(string context, CancellationToken token)
  {
    var cmd = Flux.WithArguments($"check --pre --context {context}");
    var (ExitCode, _) = await CLIRunner.RunAsync(cmd, token);
    return ExitCode;
  }

  internal static async Task<int> InstallAsync(string context, CancellationToken token)
  {
    var cmd = Flux.WithArguments($"install --context {context}");
    var (ExitCode, _) = await CLIRunner.RunAsync(cmd, token);
    return ExitCode;
  }

  internal static async Task<int> CreateSourceOCIAsync(string context, string sourceUrl, CancellationToken token)
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
    var (ExitCode, _) = await CLIRunner.RunAsync(cmd, token);
    return ExitCode;
  }
  internal static async Task<int> CreateKustomizationAsync(string context, string fluxKustomizationPath, CancellationToken token)
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
    var (ExitCode, _) = await CLIRunner.RunAsync(cmd, token);
    return ExitCode;
  }

  internal static async Task<int> PushManifestsAsync(string ociUrl, string manifestsPath, CancellationToken token)
  {
    long currentTimeEpoch = DateTime.Now.ToEpochTime();
    var pushCmd = Flux.WithArguments(
      [
        "push",
        "artifact",
        $"{ociUrl}:{currentTimeEpoch}",
        $"--path={manifestsPath}",
        $"--source={ociUrl}",
        $"--revision={currentTimeEpoch}"
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
    var (ExitCode, _) = await CLIRunner.RunAsync(pushCmd, token);
    if (ExitCode != 0)
    {
      Console.WriteLine($"✕ Failed to push manifests to {ociUrl}");
      return 1;
    }
    (ExitCode, _) = await CLIRunner.RunAsync(tagCmd, token);
    if (ExitCode != 0)
    {
      Console.WriteLine("✕ Failed to tag manifests with 'latest' tag");
      return 1;
    }
    return 0;
  }

  internal static async Task<int> ReconcileAsync(string context, CancellationToken token)
  {
    var cmd = Flux.WithArguments($"reconcile source oci flux-system --context {context}");
    var (ExitCode, _) = await CLIRunner.RunAsync(cmd, token);
    return ExitCode;
  }
}
