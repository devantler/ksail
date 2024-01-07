
using System.Runtime.InteropServices;
using CliWrap;

namespace KSail.CLIWrappers;

static class KubeconformCLIWrapper
{
  internal static Command Kubeconform
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "kubeconform_darwin_amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "kubeconform_darwin_arm64",
        (PlatformID.Unix, Architecture.X64, false) => "kubeconform_linux_amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "kubeconform_linux_arm64",
        (PlatformID.Unix, Architecture.Arm, false) => "kubeconform_linux_arm",
        _ => throw new PlatformNotSupportedException()
      };
      return Cli.Wrap($"assets/{binary}");
    }
  }
}
