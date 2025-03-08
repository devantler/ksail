
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Native;

class KSailGenNativeAccountCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly GenericPathOption _outputOption = new("./service-account.yaml");
  public KSailGenNativeAccountCommand() : base("service-account", "Generate a 'core/v1/ServiceAccount' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        try
        {
          string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? "./service-account.yaml";
          bool overwrite = context.ParseResult.RootCommandResult.GetValueForOption(CLIOptions.Generator.OverwriteOption) ?? false;
          if (overwrite)
          {
            Console.WriteLine($"✚ overwriting {outputFile}");
          }
          else
          {
            Console.WriteLine($"✚ generating {outputFile}");
          }
          KSailGenNativeAccountCommandHandler handler = new(outputFile, overwrite);
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
