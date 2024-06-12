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
        (PlatformID.Unix, Architecture.X64, true) => "age-keygen-darwin-amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "age-keygen-darwin-arm64",
        (PlatformID.Unix, Architecture.X64, false) => "age-keygen-linux-amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "age-keygen-linux-arm64",
        _ => throw new PlatformNotSupportedException($"🚨 Unsupported platform: {Environment.OSVersion.Platform} {RuntimeInformation.ProcessArchitecture}"),
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }

  internal static async Task<int> GenerateKeyAsync(string keyName, bool shouldOverwrite, CancellationToken token)
  {
    keyName = keyName.ToLowerInvariant();
    if (File.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/age/{keyName}.agekey"))
    {
      if (!shouldOverwrite)
      {
        Console.WriteLine($"✕ Key '{keyName}' already exists. Use --overwrite to replace it.");
        return 1;
      }
      File.Delete($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/age/{keyName}.agekey");
    }
    _ = Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/age");
    var cmd = AgeKeygen.WithArguments($"-o {Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/age/{keyName}.agekey");
    var (ExitCode, Result) = await CLIRunner.RunAsync(cmd, token, silent: true);
    if (ExitCode != 0)
    {
      Console.WriteLine($"✕ Failed to generate key with error: {Result[^1]}");
      return 1;
    }
    return 0;
  }

  internal static async Task<int> ShowKeyAsync(string clusterName, CancellationToken token)
  {
    clusterName = clusterName.ToLowerInvariant();
    if (!File.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/age/{clusterName}.agekey"))
    {
      Console.WriteLine($"✕ Key '{clusterName}' not found");
      return 1;
    }
    string key = await File.ReadAllTextAsync($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/age/{clusterName}.agekey", token);
    Console.WriteLine(key);
    return 0;
  }
}
