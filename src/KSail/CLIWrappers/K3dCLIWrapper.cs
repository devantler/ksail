using System.Runtime.InteropServices;
using CliWrap;

namespace KSail.CLIWrappers;

/// <summary>
/// A CLI wrapper for the 'k3d' binary.
/// </summary>
public static class K3dCLIWrapper
{
  /// <summary>
  /// The 'k3d' binary.
  /// </summary>
  /// <exception cref="PlatformNotSupportedException"></exception>
  public static Command K3d
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "k3d_darwin_amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "k3d_darwin_arm64",
        (PlatformID.Unix, Architecture.X64, false) => "k3d_linux_amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "k3d_linux_arm64",
        (PlatformID.Unix, Architecture.Arm, false) => "k3d_linux_arm",
        _ => throw new PlatformNotSupportedException()
      };
      return Cli.Wrap($"assets/{binary}");
    }
  }
}
