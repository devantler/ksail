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
        (PlatformID.Unix, Architecture.X64, true) => "k3d-darwin-amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "k3d-darwin-arm64",
        (PlatformID.Unix, Architecture.X64, false) => "k3d-linux-amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "k3d-linux-arm64",
        _ => throw new PlatformNotSupportedException($"🚨 Unsupported platform: {Environment.OSVersion.Platform} {RuntimeInformation.ProcessArchitecture}"),
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }

  internal static async Task<int> CreateClusterAsync(string clusterName, string configPath, CancellationToken token)
  {
    var cmd = K3d.WithArguments(
        [
          "cluster",
          "create",
          $"{clusterName}",
          $"--config={configPath}"
        ]
      );
    var (ExitCode, _) = await CLIRunner.RunAsync(cmd, token);
    return ExitCode;
  }

  internal static async Task<int> StartClusterAsync(string clusterName, CancellationToken token)
  {
    var cmd = K3d.WithArguments($"cluster start {clusterName}");
    var (ExitCode, _) = await CLIRunner.RunAsync(cmd, token);
    return ExitCode;
  }
  internal static async Task<int> StopClusterAsync(string clusterName, CancellationToken token)
  {
    var cmd = K3d.WithArguments($"cluster stop {clusterName}");
    var (ExitCode, _) = await CLIRunner.RunAsync(cmd, token);
    return ExitCode;
  }

  internal static async Task<int> DeleteClusterAsync(string clusterName, CancellationToken token)
  {
    var cmd = K3d.WithArguments($"cluster delete {clusterName}");
    var (ExitCode, _) = await CLIRunner.RunAsync(cmd, token);
    return ExitCode;
  }

  //TODO: Make the GetClusterAsync work with the CLIRunner.RunAsync method.
  internal static async Task<(int ExitCode, bool Result)> GetClusterAsync(string clusterName, CancellationToken token)
  {
    var cmd = K3d.WithArguments($"cluster get {clusterName}").WithValidation(CommandResultValidation.None);
    var result = await cmd.ExecuteBufferedAsync(cancellationToken: token);
    return (0, result.IsSuccess);
  }

  internal static async Task<(int ExitCode, string Result)> ListClustersAsync(CancellationToken token)
  {
    var cmd = K3d.WithArguments("cluster list");
    var (ExitCode, Result) = await CLIRunner.RunAsync(cmd, token);
    return (ExitCode, Result);
  }
}
