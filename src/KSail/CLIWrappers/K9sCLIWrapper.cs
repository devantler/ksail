using System.Runtime.InteropServices;
using CliWrap;

namespace KSail.CLIWrappers;

class K9sCLIWrapper()
{
  static Command K9s
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "k9s-darwin-amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "k9s-darwin-arm64",
        (PlatformID.Unix, Architecture.X64, false) => "k9s-linux-amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "k9s-linux-arm64",
        _ => throw new PlatformNotSupportedException($"🚨 Unsupported platform: {Environment.OSVersion.Platform} {RuntimeInformation.ProcessArchitecture}"),
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }

  internal static async Task<int> DebugClusterAsync(string kubeconfig, string? context, CancellationToken token)
  {
    var cmd = context is null ? K9s : K9s.WithArguments([$"--kubeconfig {kubeconfig}", $"--context {context}"]);
    var (ExitCode, _) = await CLIRunner.RunAsync(cmd, token);
    return ExitCode;
  }
}
