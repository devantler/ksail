using CliWrap;
using System.Runtime.InteropServices;

namespace KSail.CLIWrappers;

class AgeCLIWrapper()
{
  static Command AgeKeygen
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "age-keygen_darwin-amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "age-keygen_darwin-arm64",
        (PlatformID.Unix, Architecture.X64, false) => "age-keygen_linux-amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "age-keygen_linux-arm64",
        _ => throw new PlatformNotSupportedException($"🚨 Unsupported platform: {Environment.OSVersion.Platform} {RuntimeInformation.ProcessArchitecture}"),
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }

  internal static async Task<int> GenerateKeyAsync(CancellationToken token)
  {
    if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail"))
    {
      _ = Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail");
    }
    if (File.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/ksail_sops.agekey"))
    {
      File.Delete($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/ksail_sops.agekey");
    }
    var cmd = AgeKeygen.WithArguments($"-o {Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/ksail_sops.agekey");
    var (ExitCode, Result) = await CLIRunner.RunAsync(cmd, token, silent: true);
    if (ExitCode != 0)
    {
      Console.WriteLine($"✕ Failed to generate key with error: {Result.Last()}");
      Console.WriteLine(Result.Last());
      return 1;
    }
    return 0;
  }
}
