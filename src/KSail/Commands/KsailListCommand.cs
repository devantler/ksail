using System.CommandLine;

namespace KSail.Commands;

/// <summary>
/// The 'list' command responsible for listing running clusters.
/// </summary>
public class ListCommand : Command
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ListCommand"/> class.
  /// </summary>
  public ListCommand() : base("list", "list running clusters") => this.SetHandler(() => _ = this.InvokeAsync("--help"));
}
