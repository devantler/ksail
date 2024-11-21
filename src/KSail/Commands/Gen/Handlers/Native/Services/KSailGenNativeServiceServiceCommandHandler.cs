using Devantler.KubernetesGenerator.Native.Service;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Services;

class KSailGenNativeServiceServiceCommandHandler
{
  readonly ServiceGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1Service
    {
      ApiVersion = "networking.k8s.io/v1",
      Kind = "Service",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      Spec = new V1ServiceSpec()
      {
        Ports =
        [
          new V1ServicePort()
          {
            Name = "<name>",
            Port = 0,
            TargetPort = 0,
          },
        ],
        Selector = new Dictionary<string, string>()
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
