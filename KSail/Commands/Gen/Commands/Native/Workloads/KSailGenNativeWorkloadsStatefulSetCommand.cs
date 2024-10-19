
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Workloads;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Workloads;

class KSailGenNativeWorkloadsStatefulSetCommand : Command
{
  readonly FileOutputOption _outputOption = new("./stateful-set.yaml");
  readonly KSailGenNativeWorkloadsStatefulSetCommandHandler _handler = new();
  public KSailGenNativeWorkloadsStatefulSetCommand() : base("stateful-set", "Generate a 'apps/v1/StatefulSet' resource.")
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
