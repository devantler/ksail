using System.CommandLine;

namespace KSail.Commands;

public class SOPSCommand : Command
{
  public SOPSCommand() : base("sops", "manage KSail SOPS GPG key")
  {
  }
}
