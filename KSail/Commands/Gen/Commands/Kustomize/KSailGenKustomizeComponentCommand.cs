
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Kustomize;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Kustomize;

class KSailGenKustomizeComponentCommand : Command
{
  readonly FileOutputOption _outputOption = new("./kustomization.yaml");
  readonly KSailGenKustomizeComponentCommandHandler _handler = new();
  internal KSailGenKustomizeComponentCommand() : base("component", "Generate a 'kustomize.config.k8s.io/v1alpha1/Component' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        string outputPath = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        context.ExitCode = await _handler.HandleAsync(outputPath, context.GetCancellationToken()).ConfigureAwait(false);
      }
    );
  }
}
