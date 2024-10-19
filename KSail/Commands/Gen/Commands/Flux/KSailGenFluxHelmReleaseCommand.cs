
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Flux;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Flux;

class KSailGenFluxHelmReleaseCommand : Command
{
  readonly FileOutputOption _outputOption = new("./helm-release.yaml");
  readonly KSailGenFluxHelmReleaseCommandHandler _handler = new();
  public KSailGenFluxHelmReleaseCommand() : base("helm-release", "Generate a 'helm.toolkit.fluxcd.io/v2/HelmRelease' resource.")
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
