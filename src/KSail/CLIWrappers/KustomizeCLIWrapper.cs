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
        (PlatformID.Unix, Architecture.X64, true) => "kustomize_darwin_amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "kustomize_darwin_arm64",
        (PlatformID.Unix, Architecture.X64, false) => "kustomize_linux_amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "kustomize_linux_arm64",
        (PlatformID.Unix, Architecture.Arm, false) => "kustomize_linux_arm",
        _ => throw new PlatformNotSupportedException()
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }
}
