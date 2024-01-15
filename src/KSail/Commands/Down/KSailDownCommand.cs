using System.CommandLine;
using KSail.Commands.Down.Handlers;
using KSail.Options;

namespace KSail.Commands.Down;

sealed class KSailDownCommand : Command
{
  readonly NameOption nameOption = new("Name of the cluster to destroy") { IsRequired = true };
  internal KSailDownCommand(IConsole console) : base("down", "Destroy a K8s cluster")
  {
    AddOption(nameOption);
    this.SetHandler(new KSailDownCommandHandler(console).HandleAsync, nameOption);
  }
}
