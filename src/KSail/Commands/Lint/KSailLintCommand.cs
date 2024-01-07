using System.CommandLine;

namespace KSail.Commands.Lint;

sealed class KSailLintCommand : Command
{
  internal KSailLintCommand() : base(
   "lint", "lint manifest files"
  ) => this.SetHandler(() => _ = this.InvokeAsync("--help"));
}
