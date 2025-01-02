using Devantler.KubernetesGenerator.Native.Service;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativeIngressClassCommandHandler
{
  readonly IngressClassGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1IngressClass
    {
      ApiVersion = "networking.k8s.io/v1",
      Kind = "IngressClass",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      Spec = new V1IngressClassSpec()
      {
        Controller = "<controller>",
        Parameters = new V1IngressClassParametersReference()
        {
          Kind = "<kind>",
          Name = "<name>",
        }
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
