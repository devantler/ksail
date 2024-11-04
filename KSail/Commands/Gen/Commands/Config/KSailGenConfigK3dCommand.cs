
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Config;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Config;

class KSailGenConfigK3dCommand : Command
{
  readonly FileOutputOption _outputOption = new("./k3d-config.yaml");

  public KSailGenConfigK3dCommand() : base("k3d", "Generate a 'k3d.io/v1alpha5/Simple' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
    {
      try
      {
        string outputFile = context.ParseResult.RootCommandResult.GetValueForOption(_outputOption)!;
        var handler = new KSailGenConfigK3dCommandHandler(outputFile);
        Console.WriteLine($"✚ Generating {outputFile}");
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
      }
      catch (OperationCanceledException ex)
      {
        ExceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }
}




