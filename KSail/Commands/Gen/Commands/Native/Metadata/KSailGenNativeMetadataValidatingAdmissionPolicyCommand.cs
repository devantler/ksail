
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Metadata;
using KSail.Commands.Gen.Options;

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
        string outputPath = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        await _handler.HandleAsync(outputPath, context.GetCancellationToken()).ConfigureAwait(false);
      }
    );
  }
}
