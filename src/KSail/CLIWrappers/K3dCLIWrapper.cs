using System.Runtime.InteropServices;
using CliWrap;
using CliWrap.EventStream;

namespace KSail.CLIWrappers;

static class K3dCLIWrapper
{
  static Command K3d
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

  internal static async Task CreateClusterAsync(string name)
  {
    var cmd = K3d.WithArguments($"cluster create {name}");
    _ = await CLIRunner.RunAsync(cmd);
  }

  internal static async Task CreateClusterFromConfigAsync(string configPath)
  {
    var cmd = K3d.WithArguments($"cluster create --config {configPath}");
    _ = await CLIRunner.RunAsync(cmd);
  }

  internal static async Task DeleteClusterAsync(string name)
  {
    var cmd = K3d.WithArguments($"cluster delete {name}");
    _ = await CLIRunner.RunAsync(cmd);
  }
}
