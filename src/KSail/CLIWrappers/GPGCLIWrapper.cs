using CliWrap;
using CliWrap.Buffered;
using CliWrap.EventStream;

namespace KSail.CLIWrappers;

/// <summary>
/// A CLI wrapper for the 'gpg' binary.
/// </summary>
public static class GPGCLIWrapper
{
  /// <summary>
  /// The 'gpg' binary.
  /// </summary>
  /// <exception cref="PlatformNotSupportedException"></exception>
  public static Command GPG
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

  /// <summary>
  /// Creates a GPG key.
  /// </summary>
  /// <returns>The private key.</returns>
  public static async Task<string> CreateGPGKeyAsync()
  {
    try
    {
      await foreach (var cmdEvent in GPG.WithArguments("--batch --passphrase '' --quick-gen-key ksail default default").ListenAsync())
      {
        Console.WriteLine(cmdEvent);
      }
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
    }
    string fingerprint = (await GPG.WithArguments("--list-keys -uid ksail | grep '^      *' | tr -d ' '").ExecuteBufferedAsync()).StandardOutput;
    return (await GPG.WithArguments($"--export-secret-keys --armor {fingerprint}").ExecuteBufferedAsync()).StandardOutput;
  }
}
