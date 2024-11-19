
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Metadata;
using KSail.Commands.Gen.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataValidatingAdmissionPolicyCommand : Command
{
  readonly FileOutputOption _outputOption = new("./validating-admission-policy.yaml");
  readonly KSailGenNativeMetadataValidatingAdmissionPolicyCommandHandler _handler = new();

  public KSailGenNativeMetadataValidatingAdmissionPolicyCommand() : base("validating-admission-policy", "Generate a 'admissionregistration.k8s.io/v1/ValidatingAdmissionPolicy' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        try
        {
          Console.WriteLine($"✚ generating {outputFile}");
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
