using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Update.Handlers;
using KSail.Commands.Update.Options;
using KSail.Options;

namespace KSail.Commands.Update;

sealed class KSailUpdateCommand : Command
{
  readonly NameArgument nameArgument = new() { Arity = ArgumentArity.ExactlyOne };
  readonly ManifestsOption manifestsOption = new() { IsRequired = true };
  readonly NoLintOption noLintOption = new();
  readonly NoReconcileOption noReconcileOption = new();
  internal KSailUpdateCommand() : base(
    "update",
    "Update manifests in an OCI registry"
  )
  {
    AddArgument(nameArgument);
    AddOption(manifestsOption);
    AddOption(noLintOption);
    AddOption(noReconcileOption);
    this.SetHandler(KSailUpdateCommandHandler.HandleAsync, nameArgument, manifestsOption, noLintOption, noReconcileOption);
  }
}
