
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Config;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Config;

class KSailGenConfigKSailCommand : Command
{
  readonly FileOutputOption _fileOutputOption = new("./ksail-config.yaml");
  readonly KSailGenConfigKSailCommandHandler _handler = new();
  public KSailGenConfigKSailCommand() : base("ksail", "Generate a 'ksail.io/v1alpha1/Cluster' resource.")
  {
    AddOption(_fileOutputOption);
    this.SetHandler(async (context) =>
      {
        string outputFile = context.ParseResult.GetValueForOption(_fileOutputOption)!;
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
