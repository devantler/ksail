
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Services;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Service;

class KSailGenNativeServiceIngressCommand : Command
{
  readonly FileOutputOption _outputOption = new("./ingress.yaml");
  readonly KSailGenNativeServiceIngressCommandHandler _handler = new();
  public KSailGenNativeServiceIngressCommand() : base("ingress", "Generate a 'networking.k8s.io/v1/Ingress' resource.")
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
