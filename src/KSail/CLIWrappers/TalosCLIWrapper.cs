using System.Runtime.InteropServices;
using CliWrap;

namespace KSail.CLIWrappers;

/// <summary>
/// A CLI wrapper for the 'talosctl' binary.
/// </summary>
public static class TalosCLIWrapper
{
  /// <summary>
  /// The 'talosctl' binary.
  /// </summary>
  /// <exception cref="PlatformNotSupportedException"></exception>
  public static Command Talosctl
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "talosctl_v1.6.1_darwin_amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "talosctl_v1.6.1_darwin_arm64",
        (PlatformID.Unix, Architecture.X64, false) => "talosctl_v1.6.1_linux_amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "talosctl_v1.6.1_linux_arm64",
        (PlatformID.Unix, Architecture.Arm, false) => "talosctl_v1.6.1_linux_arm",
        _ => throw new PlatformNotSupportedException()
      };
      return Cli.Wrap($"assets/{binary}");
    }
  }
}
