
using Devantler.KubernetesGenerator.Native.Cluster;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Cluster;

class KSailGenNativeClusterAPIServiceCommandHandler
{
  readonly APIServiceGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1APIService()
    {
      ApiVersion = "apiregistration.k8s.io/v1",
      Kind = "APIService",
      Metadata = new V1ObjectMeta()
      {
        Name = "<version>.<group>",
      },
      Spec = new V1APIServiceSpec()
      {
        Service = new Apiregistrationv1ServiceReference()
        {
          Name = "<service-name>",
          NamespaceProperty = "<namespace>",
        },
        Group = "<group>",
        Version = "<version>",
        GroupPriorityMinimum = 100,
        VersionPriority = 100
      }
    };

    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
