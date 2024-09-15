
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Metadata;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataMutatingWebhookConfigurationCommand : Command
{
  readonly FileOutputOption _outputOption = new("./mutating-webhook-configuration.yaml");
  readonly KSailGenNativeMetadataMutatingWebhookConfigurationCommandHandler _handler = new();

  public KSailGenNativeMetadataMutatingWebhookConfigurationCommand() : base("mutating-webhook-configuration", "Generate a 'admissionregistration.k8s.io/v1/MutatingWebhookConfiguration' resource.")
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
