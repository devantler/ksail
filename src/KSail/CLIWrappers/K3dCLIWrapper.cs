using System.Runtime.InteropServices;
using CliWrap;
using CliWrap.EventStream;

namespace KSail.CLIWrappers;

/// <summary>
/// A CLI wrapper for the 'k3d' binary.
/// </summary>
public static class K3dCLIWrapper
{
  /// <summary>
  /// The 'k3d' binary.
  /// </summary>
  /// <exception cref="PlatformNotSupportedException"></exception>
  public static Command K3d
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "k3d_darwin_amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "k3d_darwin_arm64",
        (PlatformID.Unix, Architecture.X64, false) => "k3d_linux_amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "k3d_linux_arm64",
        (PlatformID.Unix, Architecture.Arm, false) => "k3d_linux_arm",
        _ => throw new PlatformNotSupportedException()
      };
      return Cli.Wrap($"assets/{binary}");
    }
  }

  /// <summary>
  /// Creates a K3d cluster with a specified name.
  /// </summary>
  /// <param name="name">The name of the cluster.</param>
  public static async Task CreateClusterAsync(string name)
  {
    await foreach (var cmdEvent in K3d.WithArguments($"cluster create {name}").ListenAsync())
    {
      Console.WriteLine(cmdEvent);
    }
  }

  /// <summary>
  /// Creates a K3d cluster from a specified configuration file.
  /// </summary>
  /// <param name="configPath">The path to the configuration file.</param>
  public static async Task CreateClusterFromConfigAsync(string configPath)
  {
    await foreach (var cmdEvent in K3d.WithArguments($"cluster create --config {configPath}").ListenAsync())
    {
      Console.WriteLine(cmdEvent);
    }
  }

  /// <summary>
  /// Deletes a K3d cluster with a specified name.
  /// </summary>
  /// <param name="name">The name of the cluster.</param>
  public static async Task DeleteClusterAsync(string name)
  {
    await foreach (var cmdEvent in K3d.WithArguments($"cluster delete {name}").ListenAsync())
    {
      Console.WriteLine(cmdEvent);
    }
  }
}
