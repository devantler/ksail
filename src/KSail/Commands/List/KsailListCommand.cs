using System.CommandLine;

namespace KSail.Commands.List;

sealed class ListCommand : Command
{
  internal ListCommand() : base("list", "list running clusters") => this.SetHandler(() => _ = this.InvokeAsync("--help"));
}
