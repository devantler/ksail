using System.CommandLine;

namespace KSail.Commands.Validate;

sealed class ValidateCommand : Command
{
  internal ValidateCommand() : base(
   "validate", "validate manifests"
 ) => this.SetHandler(() => _ = this.InvokeAsync("--help"));
}
