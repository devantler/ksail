using CliWrap;
using System.CommandLine;
using System.Runtime.InteropServices;

namespace KSail.CLIWrappers;

class SOPSCLIWrapper(IConsole console)
{
  readonly CLIRunner cliRunner = new(console);

  static CliWrap.Command SOPS
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "sops_darwin-amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "sops_darwin-arm64",
        (PlatformID.Unix, Architecture.X64, false) => "sops_linux-amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "sops_linux-arm64",
        _ => throw new PlatformNotSupportedException()
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }

  internal async Task DecryptAsync(string decrypt)
  {
    if (!File.Exists(decrypt))
    {
      console.WriteLine($"✖ File '{decrypt}' does not exist");
      Environment.Exit(1);
    }
    var cmd = SOPS.WithArguments($"-d -i {decrypt}");
    _ = await cliRunner.RunAsync(cmd, silent: true);
  }
  internal async Task EncryptAsync(string encrypt)
  {
    if (!File.Exists(encrypt))
    {
      console.WriteLine($"✖ File '{encrypt}' does not exist");
      Environment.Exit(1);
    }
    var cmd = SOPS.WithArguments($"-e -i {encrypt}");
    _ = await cliRunner.RunAsync(cmd, silent: true);
  }
}
