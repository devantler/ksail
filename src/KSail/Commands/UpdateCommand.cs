using System.CommandLine;

namespace KSail.Commands;

public class UpdateCommand : Command
{
  public UpdateCommand() : base("update", "update a K8s cluster in Docker by pushing manifests in a specified directory to a local OCI registry")
  {
  }
}
