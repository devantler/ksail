using System.CommandLine;

namespace KSail.Commands;

public class VerifyCommand : Command
{
  public VerifyCommand() : base("verify", "verify reconciliation of kustomizations in a K8s cluster")
  {
  }
}
