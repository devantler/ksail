using System.CommandLine;

namespace KSail.Presentation.Commands;

/// <summary>
/// The 'update' command responsible for updating K8s clusters by pushing manifests in a specified directory to a local OCI registry.
/// </summary>
public class UpdateCommand : Command
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UpdateCommand"/> class.
  /// </summary>
  public UpdateCommand() : base(
    "update",
    "update a K8s cluster"
  ) => this.SetHandler(() => _ = this.InvokeAsync("--help"));
}