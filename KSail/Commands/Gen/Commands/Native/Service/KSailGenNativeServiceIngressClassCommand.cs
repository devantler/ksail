
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Services;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Service;

class KSailGenNativeServiceIngressClassCommand : Command
{
  readonly FileOutputOption _outputOption = new("./ingress-class.yaml");
  readonly KSailGenNativeServiceIngressClassCommandHandler _handler = new();
  public KSailGenNativeServiceIngressClassCommand() : base("ingress-class", "Generate a 'networking.k8s.io/v1/IngressClass' resource.")
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
