using System.Runtime.InteropServices;
using CliWrap;
using CliWrap.Buffered;

namespace KSail.CLIWrappers;

class K3dCLIWrapper()
{
  static Command K3d
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "k3d_darwin-amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "k3d_darwin-arm64",
        (PlatformID.Unix, Architecture.X64, false) => "k3d_linux-amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "k3d_linux-arm64",
        _ => throw new PlatformNotSupportedException()
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }

  internal static async
  Task
CreateClusterAsync(string name, string configPath)
  {
    var cmd = K3d.WithArguments(
        [
          "cluster",
          "create",
          $"{name}",
          $"--config={configPath}",
          $"--registry-config={AppContext.BaseDirectory}assets/k3d/registry-config.yaml"
        ]
      );
    _ = await CLIRunner.RunAsync(cmd);
  }

  internal static async Task StartClusterAsync(string name)
  {
    var cmd = K3d.WithArguments($"cluster start {name}");
    _ = await CLIRunner.RunAsync(cmd);
  }
  internal static async Task StopClusterAsync(string name)
  {
    var cmd = K3d.WithArguments($"cluster stop {name}");
    _ = await CLIRunner.RunAsync(cmd);
  }

  internal static async Task DeleteClusterAsync(string name)
  {
    var cmd = K3d.WithArguments($"cluster delete {name}");
    _ = await CLIRunner.RunAsync(cmd);
  }

  internal static async Task<bool> GetClusterAsync(string name)
  {
    var cmd = K3d.WithArguments($"cluster get {name}").WithValidation(CommandResultValidation.None);
    var result = await cmd.ExecuteBufferedAsync();
    return result.IsSuccess;
  }

  internal static Task<string> ListClustersAsync()
  {
    var cmd = K3d.WithArguments("cluster list");
    return CLIRunner.RunAsync(cmd);
  }
}
