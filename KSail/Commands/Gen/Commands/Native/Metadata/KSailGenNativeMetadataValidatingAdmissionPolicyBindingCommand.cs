
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
