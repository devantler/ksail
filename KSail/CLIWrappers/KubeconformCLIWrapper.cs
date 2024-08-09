using System.Runtime.InteropServices;
using CliWrap;
using Devantler.CLIRunner;

namespace KSail.CLIWrappers;

class KubeconformCLIWrapper()
{
  internal static Command Kubeconform
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "kubeconform-darwin-amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "kubeconform-darwin-arm64",
        (PlatformID.Unix, Architecture.X64, false) => "kubeconform-linux-amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "kubeconform-linux-arm64",
        _ => throw new PlatformNotSupportedException($"🚨 Unsupported platform: {Environment.OSVersion.Platform} {RuntimeInformation.ProcessArchitecture}"),
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }

  internal static async Task<int> RunAsync(string[] kubeconformFlags, string[] kubeconformConfig, string manifest, CancellationToken token)
  {
    var arguments = kubeconformFlags.Concat(kubeconformConfig).Concat([manifest]);
    var cmd = Kubeconform.WithArguments(arguments);
    var (exitCode, _) = await CLIRunner.RunAsync(cmd, token, silent: true).ConfigureAwait(false);
    return exitCode;
  }
}
