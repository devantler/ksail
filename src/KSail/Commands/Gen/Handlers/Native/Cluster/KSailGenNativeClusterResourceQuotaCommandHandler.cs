using Devantler.KubernetesGenerator.Native.Cluster;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Cluster;

class KSailGenNativeClusterResourceQuotaCommandHandler
{
  readonly ResourceQuotaGenerator _generator = new();

  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1ResourceQuota()
    {
      ApiVersion = "v1",
      Kind = "ResourceQuota",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      Spec = new V1ResourceQuotaSpec()
      {
        Hard = new Dictionary<string, ResourceQuantity>(),
        ScopeSelector = new V1ScopeSelector()
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
