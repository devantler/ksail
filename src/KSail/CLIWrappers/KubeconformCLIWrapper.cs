using System.Runtime.InteropServices;
using CliWrap;

namespace KSail.CLIWrappers;

class KubeconformCLIWrapper()
{
  internal static Command Kubeconform
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "kubeconform_darwin-amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "kubeconform_darwin-arm64",
        (PlatformID.Unix, Architecture.X64, false) => "kubeconform_linux-amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "kubeconform_linux-arm64",
        _ => throw new PlatformNotSupportedException()
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }

  internal static async Task Run(string[] kubeconformFlags, string[] kubeconformConfig, string manifest)
  {
    var cmd = Kubeconform.WithArguments(kubeconformFlags.Concat(kubeconformConfig).Concat(new[] { manifest }).ToArray());
    _ = await CLIRunner.RunAsync(cmd, silent: true);
  }
}
