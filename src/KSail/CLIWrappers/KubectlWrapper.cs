
using System.Runtime.InteropServices;
using CliWrap;

namespace KSail.CLIWrappers;

static class KubectlWrapper
{
  internal static Command Kubectl
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "kubectl_darwin_amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "kubectl_darwin_arm64",
        (PlatformID.Unix, Architecture.X64, false) => "kubectl_linux_amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "kubectl_linux_arm64",
        (PlatformID.Unix, Architecture.Arm, false) => "kubectl_linux_arm",
        _ => throw new PlatformNotSupportedException()
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }
}
