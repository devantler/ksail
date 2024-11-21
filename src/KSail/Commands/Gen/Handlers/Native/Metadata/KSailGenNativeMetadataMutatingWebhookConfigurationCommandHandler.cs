using Devantler.KubernetesGenerator.Native.Metadata;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Metadata;

class KSailGenNativeMetadataMutatingWebhookConfigurationCommandHandler
{
  readonly MutatingWebhookConfigurationGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1MutatingWebhookConfiguration()
    {
      ApiVersion = "admissionregistration.k8s.io/v1",
      Kind = "MutatingWebhookConfiguration",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>"
      },
      Webhooks = []
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
