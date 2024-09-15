using Devantler.KubernetesGenerator.Native.Metadata;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Metadata;

class KSailGenNativeMetadataValidatingAdmissionPolicyCommandHandler
{
  readonly ValidatingAdmissionPolicyGenerator _generator = new();
  internal async Task HandleAsync(string outputPath, CancellationToken cancellationToken = default)
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
    await _generator.GenerateAsync(model, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
