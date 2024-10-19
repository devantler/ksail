
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Kustomize;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Kustomize;

class KSailGenKustomizeKustomizationCommand : Command
{
  readonly FileOutputOption _outputOption = new("./kustomization.yaml");
  readonly KSailGenKustomizeKustomizationCommandHandler _handler = new();
  public KSailGenKustomizeKustomizationCommand() : base("kustomization", "Generate a 'kustomize.config.k8s.io/v1beta1/Kustomization' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        try
        {
          Console.WriteLine($"✚ Generating {outputFile}");
          context.ExitCode = await _handler.HandleAsync(outputFile, context.GetCancellationToken()).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
          Console.WriteLine("✕ Operation was canceled by the user.");
          context.ExitCode = 1;
        }
      }
    );
  }
}

