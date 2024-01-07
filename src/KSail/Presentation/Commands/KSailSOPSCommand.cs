using System.CommandLine;

namespace KSail.Presentation.Commands;

sealed class SOPSCommand : Command
{
  internal SOPSCommand() : base("sops", "manage SOPS GPG key") => this.SetHandler(() => _ = this.InvokeAsync("--help"));
}
