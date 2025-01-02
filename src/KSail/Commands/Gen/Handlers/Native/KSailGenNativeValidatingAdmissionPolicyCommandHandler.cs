using Devantler.KubernetesGenerator.Native.Metadata;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativeValidatingAdmissionPolicyCommandHandler
{
  readonly ValidatingAdmissionPolicyGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1ValidatingAdmissionPolicy
    {
      ApiVersion = "admissionregistration.k8s.io/v1",
      Kind = "ValidatingAdmissionPolicy",
      Metadata = new V1ObjectMeta
      {
        Name = "<name>"
      },
      Spec = new V1ValidatingAdmissionPolicySpec
      {
        AuditAnnotations = [],
        Validations = []
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
