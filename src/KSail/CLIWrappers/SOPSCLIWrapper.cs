using CliWrap;
using System.Runtime.InteropServices;

namespace KSail.CLIWrappers;

class SOPSCLIWrapper()
{
  static Command SOPS
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "sops-darwin-amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "sops-darwin-arm64",
        (PlatformID.Unix, Architecture.X64, false) => "sops-linux-amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "sops-linux-arm64",
        _ => throw new PlatformNotSupportedException($"🚨 Unsupported platform: {Environment.OSVersion.Platform} {RuntimeInformation.ProcessArchitecture}"),
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }

  internal static async Task<int> DecryptAsync(string decrypt, string masterKeyPath, CancellationToken token)
  {
    if (!File.Exists(decrypt))
    {
      Console.WriteLine($"✕ File '{decrypt}' does not exist");
      return 1;
    }
    Environment.SetEnvironmentVariable("SOPS_AGE_KEY_FILE", masterKeyPath);
    var cmd = SOPS.WithArguments($"-d -i {decrypt}");
    var (ExitCode, _) = await CLIRunner.RunAsync(cmd, token, silent: true);
    Environment.SetEnvironmentVariable("SOPS_AGE_KEY_FILE", null);
    return ExitCode;
  }
  internal static async Task<int> EncryptAsync(string encrypt, string masterKeyPath, CancellationToken token)
  {
    if (!File.Exists(encrypt))
    {
      Console.WriteLine($"✕ File '{encrypt}' does not exist");
      return 1;
    }
    Environment.SetEnvironmentVariable("SOPS_AGE_KEY_FILE", masterKeyPath);
    var cmd = SOPS.WithArguments($"-e -i {encrypt}");
    var (ExitCode, _) = await CLIRunner.RunAsync(cmd, token, silent: true);
    Environment.SetEnvironmentVariable("SOPS_AGE_KEY_FILE", null);
    return ExitCode;
  }
}
