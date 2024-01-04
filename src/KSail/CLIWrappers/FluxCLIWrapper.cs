using CliWrap;
using System.Runtime.InteropServices;

namespace KSail.CLIWrappers;
/// <summary>
/// A CLI wrapper for the 'flux' binary.
/// </summary>
public static class FluxCLIWrapper
{
  /// <summary>
  /// The 'flux' binary.
  /// </summary>
  /// <exception cref="PlatformNotSupportedException"></exception>
  public static Command Flux
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
      return Cli.Wrap($"assets/{binary}");
    }
  }
}
