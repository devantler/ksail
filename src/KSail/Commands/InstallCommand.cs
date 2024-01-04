using System.CommandLine;

namespace KSail.Commands;

/// <summary>
/// The 'install' command responsible for installing system dependencies for KSail.
/// </summary>
public class InstallCommand : Command
{
  /// <summary>
  /// Initializes a new instance of the <see cref="InstallCommand"/> class.
  /// </summary>
  public InstallCommand() : base("install", "install system dependencies for KSail")
  {
  }
}
