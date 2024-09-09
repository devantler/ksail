
using System.CommandLine;
using KSail.Commands.Gen.Handlers;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands;

class KSailGenKustomizeComponentCommand : Command
{
  readonly FileOutputOption _outputOption = new() { IsRequired = true };
  readonly KSailGenKustomizeComponentCommandHandler _handler = new();
  internal KSailGenKustomizeComponentCommand() : base("kustomize-component", "Generate a Kustomize Component.")
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
