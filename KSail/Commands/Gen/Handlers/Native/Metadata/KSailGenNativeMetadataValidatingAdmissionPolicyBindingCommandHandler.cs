using Devantler.KubernetesGenerator.Native.Metadata;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Metadata;

class KSailGenNativeMetadataValidatingAdmissionPolicyBindingCommandHandler
{
  readonly ValidatingAdmissionPolicyBindingGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1ValidatingAdmissionPolicyBinding
    {
      ApiVersion = "admissionregistration.k8s.io/v1",
      Kind = "ValidatingAdmissionPolicyBinding",
      Metadata = new V1ObjectMeta
      {
        Name = "<name>"
      },
      Spec = new V1ValidatingAdmissionPolicyBindingSpec
      {
        PolicyName = "<policyName>",
        ParamRef = new V1ParamRef
        {
          Name = "<name>",
          NamespaceProperty = "<namespace>",
        },
        ValidationActions = [],
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
