
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Metadata;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataPriorityClassCommand : Command
{
  readonly FileOutputOption _outputOption = new("./priority-class.yaml");
  readonly KSailGenNativeMetadataPriorityClassCommandHandler _handler = new();

  public KSailGenNativeMetadataPriorityClassCommand() : base("priority-class", "Generate a 'scheduling.k8s.io/v1/PriorityClass' resource.")
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
