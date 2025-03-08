
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Kustomize;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Kustomize;

class KSailGenKustomizeComponentCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly GenericPathOption _outputOption = new("./kustomization.yaml");
  internal KSailGenKustomizeComponentCommand() : base("component", "Generate a 'kustomize.config.k8s.io/v1alpha1/Component' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        try
        {
          string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? "./kustomization.yaml";
          bool overwrite = context.ParseResult.RootCommandResult.GetValueForOption(CLIOptions.Generator.OverwriteOption) ?? false;
          Console.WriteLine(File.Exists(outputFile) ? (overwrite ?
            $"✚ overwriting '{outputFile}'" :
            $"✔ skipping '{outputFile}', as it already exists.") :
            $"✚ generating '{outputFile}'");
          var handler = new KSailGenKustomizeComponentCommandHandler(outputFile, overwrite);
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
