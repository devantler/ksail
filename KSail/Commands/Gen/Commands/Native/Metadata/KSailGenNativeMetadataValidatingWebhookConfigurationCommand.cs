
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Metadata;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataValidatingWebhookConfigurationCommand : Command
{
  readonly FileOutputOption _outputOption = new("./validating-webhook-configuration.yaml");
  readonly KSailGenNativeMetadataValidatingWebhookConfigurationCommandHandler _handler = new();

  public KSailGenNativeMetadataValidatingWebhookConfigurationCommand() : base("validating-webhook-configuration", "Generate a 'admissionregistration.k8s.io/v1/ValidatingWebhookConfiguration' resource.")
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
