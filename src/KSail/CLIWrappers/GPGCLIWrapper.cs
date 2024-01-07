using CliWrap;

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

  internal static async Task CreateGPGKeyAsync()
  {
    var gpgGenKeyCmd = GPG.WithArguments("--batch --passphrase '' --quick-gen-key ksail default default");
    _ = await CLIRunner.RunAsync(gpgGenKeyCmd, CommandResultValidation.None);
  }

  internal static async Task<string> GetFingerprintAsync()
  {
    var listKSailKeyCmd = GPG.WithArguments("--list-keys -uid ksail");
    string listKSailKeyCmdResult = await CLIRunner.RunAsync(listKSailKeyCmd);
    string? fingerprint = listKSailKeyCmdResult.Split('\n')[1]?.Trim();
    Console.WriteLine(fingerprint);
    return string.IsNullOrEmpty(fingerprint)
      ? throw new InvalidOperationException("🚨 Could not find the fingerprint of the newly created GPG key.")
      : fingerprint;
  }

  internal static async Task<string> ExportPublicKeyAsync()
  {
    string fingerprint = await GetFingerprintAsync();
    var exportPublicKeyCmd = GPG.WithArguments($"--export --armor {fingerprint}");
    return await CLIRunner.RunAsync(exportPublicKeyCmd);
  }

  internal static async Task<string> ExportPrivateKeyAsync()
  {
    string fingerprint = await GetFingerprintAsync();
    var exportPrivateKeyCmd = GPG.WithArguments($"--export-secret-keys --armor {fingerprint}");
    return await CLIRunner.RunAsync(exportPrivateKeyCmd);
  }
}
