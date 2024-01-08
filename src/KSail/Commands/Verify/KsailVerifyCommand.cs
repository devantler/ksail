using System.CommandLine;

namespace KSail.Commands.Verify;

sealed class VerifyCommand : Command
{
  internal VerifyCommand() : base(
   "verify", "verify reconciliation"
 ) => this.SetHandler(() => _ = this.InvokeAsync("--help"));
}
