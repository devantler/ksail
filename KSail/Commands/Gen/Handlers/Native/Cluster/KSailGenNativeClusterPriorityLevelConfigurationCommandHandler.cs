using Devantler.KubernetesGenerator.Native.Cluster;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Cluster;

class KSailGenNativeClusterPriorityLevelConfigurationCommandHandler
{
  readonly PriorityLevelConfigurationGenerator _generator = new();

  internal async Task HandleAsync(string outputPath, CancellationToken cancellationToken = default)
  {
    var model = new V1PriorityLevelConfiguration()
    {
      ApiVersion = "flowcontrol.apiserver.k8s.io/v1",
      Kind = "PriorityLevelConfiguration",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      Spec = new V1PriorityLevelConfigurationSpec()
    };
    await _generator.GenerateAsync(model, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
