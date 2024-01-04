using System.CommandLine;

namespace KSail.Commands;

/// <summary>
/// The 'verify' command responsible for verifying reconciliation of kustomizations in a K8s cluster.
/// </summary>
public class VerifyCommand : Command
{
  /// <summary>
  /// Initializes a new instance of the <see cref="VerifyCommand"/> class.
  /// </summary>
  public VerifyCommand() : base(
    "verify", "verify reconciliation of kustomizations in a K8s cluster"
  ) => this.SetHandler(() => _ = this.InvokeAsync("--help"));
}
