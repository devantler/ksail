using System.CommandLine;
using KSail.Commands.Check.Handlers;
using KSail.Commands.Check.Options;
using KSail.Options;

namespace KSail.Commands.Check;

sealed class KSailCheckCommand : Command
{
  readonly NameOption nameOption = new("The name of the cluster to check.") { IsRequired = true };

  readonly KustomizationsOption kustomizationsOption = new() { IsRequired = true };

  public KSailCheckCommand() : base("check", "Check the status of the cluster.")
  {
    this.SetHandler((name) =>
    {
      KSailCheckHandler.HandleAsync(name, kustomizationsOption);
    }, nameOption);
  }
}
