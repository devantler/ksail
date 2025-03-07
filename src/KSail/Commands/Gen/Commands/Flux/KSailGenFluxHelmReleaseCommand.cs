
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Flux;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Flux;

class KSailGenFluxHelmReleaseCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly GenericPathOption _outputOption = new("./helm-release.yaml");
  public KSailGenFluxHelmReleaseCommand() : base("helm-release", "Generate a 'helm.toolkit.fluxcd.io/v2/HelmRelease' resource.")
  {
    AddOption(_outputOption);

    this.SetHandler(async (context) =>
      {
        try
        {
          string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? "./helm-release.yaml";
          bool overwrite = context.ParseResult.RootCommandResult.GetValueForOption(CLIOptions.Generator.OverwriteOption) ?? false;
          Console.WriteLine($"✚ generating {outputFile}");
          var handler = new KSailGenFluxHelmReleaseCommandHandler(outputFile, overwrite);
          context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
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
