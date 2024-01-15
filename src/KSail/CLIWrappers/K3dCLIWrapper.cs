using System.CommandLine;
using System.Runtime.InteropServices;
using CliWrap;
using CliWrap.Buffered;

namespace KSail.CLIWrappers;

class K3dCLIWrapper(IConsole console)
{
  readonly CLIRunner cliRunner = new(console);
  static CliWrap.Command K3d
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

  internal async Task CreateClusterAsync(string name, bool pullThroughRegistries)
  {
    var cmd = pullThroughRegistries
      ? K3d.WithArguments(
        [
          "cluster",
          "create",
          $"{name}",
          $"--registry-config={AppContext.BaseDirectory}assets/k3d/registry-config.yaml"
        ]
      )
      : K3d.WithArguments($"cluster create {name}");
    _ = await cliRunner.RunAsync(cmd);
  }

  internal async Task CreateClusterFromConfigAsync(string configPath)
  {
    var cmd = K3d.WithArguments($"cluster create --config {configPath}");
    _ = await cliRunner.RunAsync(cmd);
  }

  internal async Task DeleteClusterAsync(string name)
  {
    var cmd = K3d.WithArguments($"cluster delete {name}");
    _ = await cliRunner.RunAsync(cmd);
  }

  internal static async Task<bool> GetClusterAsync(string name)
  {
    var cmd = K3d.WithArguments($"cluster get {name}").WithValidation(CommandResultValidation.None);
    var result = await cmd.ExecuteBufferedAsync();
    return result.ExitCode == 0;
  }

  internal async Task ListClustersAsync()
  {
    var cmd = K3d.WithArguments("cluster list");
    _ = await cliRunner.RunAsync(cmd);
  }
}
