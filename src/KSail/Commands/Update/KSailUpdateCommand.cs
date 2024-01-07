using System.CommandLine;

namespace KSail.Commands.Update;

sealed class UpdateCommand : Command
{
  internal UpdateCommand() : base(
   "update",
   "update a K8s cluster"
 ) => this.SetHandler(() => _ = this.InvokeAsync("--help"));
}
