
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Kustomize;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Kustomize;

class KSailGenKustomizeKustomizationCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly GenericPathOption _outputOption = new("./kustomization.yaml");
  readonly KSailGenKustomizeKustomizationCommandHandler _handler = new();
  public KSailGenKustomizeKustomizationCommand() : base("kustomization", "Generate a 'kustomize.config.k8s.io/v1beta1/Kustomization' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        try
        {
          Console.WriteLine($"✚ generating {outputFile}");
          context.ExitCode = await _handler.HandleAsync(outputFile, context.GetCancellationToken()).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
          _ = _exceptionHandler.HandleException(ex);
          context.ExitCode = 1;
        }
      }
    );
  }
}

