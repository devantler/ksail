
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Metadata;
using KSail.Commands.Gen.Options;
using KSail.Utils;

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
        string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        try
        {
          Console.WriteLine($"✚ Generating {outputFile}");
          context.ExitCode = await _handler.HandleAsync(outputFile, context.GetCancellationToken()).ConfigureAwait(false);
        }
        catch (OperationCanceledException ex)
        {
          ExceptionHandler.HandleException(ex);
          context.ExitCode = 1;
        }
      }
    );
  }
}
