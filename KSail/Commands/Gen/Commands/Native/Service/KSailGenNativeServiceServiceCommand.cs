
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Services;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Service;

class KSailGenNativeServiceServiceCommand : Command
{
  readonly FileOutputOption _outputOption = new("./service.yaml");
  readonly KSailGenNativeServiceServiceCommandHandler _handler = new();
  public KSailGenNativeServiceServiceCommand() : base("service", "Generate a 'core/v1/Service' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        string outputPath = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        await _handler.HandleAsync(outputPath, context.GetCancellationToken()).ConfigureAwait(false);
      }
    );
  }
}
