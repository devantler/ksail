
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
