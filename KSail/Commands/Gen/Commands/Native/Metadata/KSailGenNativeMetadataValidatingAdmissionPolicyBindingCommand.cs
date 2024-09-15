
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Metadata;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataValidatingAdmissionPolicyBindingCommand : Command
{
  readonly FileOutputOption _outputOption = new("./validating-admission-policy-binding.yaml");
  readonly KSailGenNativeMetadataValidatingAdmissionPolicyBindingCommandHandler _handler = new();

  public KSailGenNativeMetadataValidatingAdmissionPolicyBindingCommand() : base("validating-admission-policy-binding", "Generate a 'admissionregistration.k8s.io/v1/ValidatingAdmissionPolicyBinding' resource.")
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
