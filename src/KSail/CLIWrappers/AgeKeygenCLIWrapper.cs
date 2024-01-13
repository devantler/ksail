using CliWrap;
using System.Runtime.InteropServices;

namespace KSail.CLIWrappers;

static class AgeKeygenCLIWrapper
{
  static Command AgeKeygen
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "age_darwin_amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "age_darwin_arm64",
        (PlatformID.Unix, Architecture.X64, false) => "age_linux_amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "age_linux_arm64",
        _ => throw new PlatformNotSupportedException()
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}/age-keygen");
    }
  }

  internal static async Task GenerateKeyAsync()
  {
    var cmd = AgeKeygen.WithArguments("-o ksail_sops.agekey");
    _ = await CLIRunner.RunAsync(cmd);
  }
}
