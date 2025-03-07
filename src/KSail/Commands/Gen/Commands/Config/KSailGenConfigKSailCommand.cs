
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Config;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Config;

class KSailGenConfigKSailCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly GenericPathOption _outputOption = new("./ksail-config.yaml");
  public KSailGenConfigKSailCommand() : base("ksail", "Generate a 'ksail.io/v1alpha1/Cluster' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        try
        {
          string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? "./ksail-config.yaml";
          bool overwrite = context.ParseResult.RootCommandResult.GetValueForOption(CLIOptions.Generator.OverwriteOption) ?? false;
          Console.WriteLine($"✚ generating {outputFile}");
          var handler = new KSailGenConfigKSailCommandHandler(outputFile, overwrite);
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
