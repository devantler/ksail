using CliWrap;
using CliWrap.Buffered;
using CliWrap.EventStream;

namespace KSail.CLIWrappers;

static class GPGCLIWrapper
{
  static Command GPG
  {
    get
    {
      var gpg = Cli.Wrap("gpg");
      try
      {
        _ = gpg.WithArguments("-h").ExecuteAsync().GetAwaiter().GetResult();
      }
      catch (Exception)
      {
        throw new PlatformNotSupportedException("🚨 The 'gpg' binary is not installed on this system. Please install it and try again.");
      }
      return gpg;
    }
  }

  internal static async Task<string> CreateGPGKeyAsync()
  {
    await foreach (
      var cmdEvent in GPG.WithArguments(
        "--batch --passphrase '' --quick-gen-key ksail default default"
      ).WithValidation(
        CommandResultValidation.None
      ).ListenAsync()
    )
    {
      Console.WriteLine(cmdEvent);
    }

    var listKeysResult = await GPG.WithArguments("--list-keys -uid ksail").ExecuteBufferedAsync();
    string? fingerprint = listKeysResult.StandardOutput.Split('\n')[1]?.Trim();
    if (string.IsNullOrEmpty(fingerprint))
    {
      throw new InvalidOperationException("🚨 Could not find the fingerprint of the newly created GPG key.");
    }

    var exportResult = await GPG.WithArguments($"--export-secret-keys --armor {fingerprint}").ExecuteBufferedAsync();
    return exportResult.StandardOutput;
  }
}
