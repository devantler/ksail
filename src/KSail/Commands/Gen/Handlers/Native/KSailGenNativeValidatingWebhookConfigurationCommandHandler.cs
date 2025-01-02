using Devantler.KubernetesGenerator.Native.Metadata;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativeValidatingWebhookConfigurationCommandHandler
{
  readonly ValidatingWebhookConfigurationGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1ValidatingWebhookConfiguration
    {
      ApiVersion = "admissionregistration.k8s.io/v1",
      Kind = "ValidatingWebhookConfiguration",
      Metadata = new V1ObjectMeta
      {
        Name = "<name>"
      },
      Webhooks = []
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
