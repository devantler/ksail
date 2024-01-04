
using System.Runtime.InteropServices;
using CliWrap;

namespace KSail.CLIWrappers;

/// <summary>
/// A CLI wrapper for the 'kubeconform' binary.
/// </summary>
public static class KubeconformCLIWrapper
{
  /// <summary>
  /// The 'kubeconform' binary.
  /// </summary>
  /// <exception cref="PlatformNotSupportedException"></exception>
  public static Command Kubeconform
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "kubeconform_v0.6.4_darwin_amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "kubeconform_v0.6.4_darwin_arm64",
        (PlatformID.Unix, Architecture.X64, false) => "kubeconform_v0.6.4_linux_amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "kubeconform_v0.6.4_linux_arm64",
        (PlatformID.Unix, Architecture.Arm, false) => "kubeconform_v0.6.4_linux_arm",
        _ => throw new PlatformNotSupportedException()
      };
      return Cli.Wrap($"assets/{binary}");
    }
  }
}
