using Devantler.KubernetesGenerator.Native.Cluster;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Cluster;

class KSailGenNativeClusterIPAddressCommandHandler
{
  readonly IPAddressGenerator _generator = new();
  internal async Task HandleAsync(string outputPath, CancellationToken cancellationToken)
  {
    var model = new V1beta1IPAddress()
    {
      ApiVersion = "networking.k8s.io/v1beta1",
      Kind = "IPAddress",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      Spec = new V1beta1IPAddressSpec()
      {
        ParentRef = new V1beta1ParentReference()
        {
          Name = "<parent-name>",
          NamespaceProperty = "<parent-namespace>",
          Resource = "<parent-resource>",
          Group = "<parent-group>"
        }
      }
    };
    await _generator.GenerateAsync(model, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
