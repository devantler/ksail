using System.Runtime.InteropServices;
using CliWrap;

namespace KSail.CLIWrappers;

static class KustomizeCLIWrapper
{
  internal static Command Kustomize
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "kustomize-darwin-amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "kustomize-darwin-arm64",
        (PlatformID.Unix, Architecture.X64, false) => "kustomize-linux-amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "kustomize-linux-arm64",
        _ => throw new PlatformNotSupportedException($"🚨 Unsupported platform: {Environment.OSVersion.Platform} {RuntimeInformation.ProcessArchitecture}"),
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }
}
