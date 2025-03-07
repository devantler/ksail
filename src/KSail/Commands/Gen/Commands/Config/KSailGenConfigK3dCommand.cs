
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Config;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Config;

class KSailGenConfigK3dCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly GenericPathOption _outputOption = new("./k3d-config.yaml");

  public KSailGenConfigK3dCommand() : base("k3d", "Generate a 'k3d.io/v1alpha5/Simple' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
    {
      try
      {
        string outputFile = context.ParseResult.RootCommandResult.GetValueForOption(_outputOption) ?? "./k3d-config.yaml";
        bool overwrite = context.ParseResult.RootCommandResult.GetValueForOption(CLIOptions.Generator.OverwriteOption) ?? false;
        var handler = new KSailGenConfigK3dCommandHandler(outputFile, overwrite);
        Console.WriteLine($"✚ generating {outputFile}");
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }
}




