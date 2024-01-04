using System.CommandLine;

namespace KSail.Commands;

/// <summary>
/// The 'validate' command responsible for validating manifests in a specified directory.
/// </summary>
public class ValidateCommand : Command
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ValidateCommand"/> class.
  /// </summary>
  public ValidateCommand() : base(
    "validate", "validate manifests in a specified directory"
  ) => this.SetHandler(() => _ = this.InvokeAsync("--help"));
}
