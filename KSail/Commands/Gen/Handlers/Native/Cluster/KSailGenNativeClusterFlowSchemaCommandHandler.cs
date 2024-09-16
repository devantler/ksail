using Devantler.KubernetesGenerator.Native.Cluster;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Cluster;

class KSailGenNativeClusterFlowSchemaCommandHandler
{
  readonly FlowSchemaGenerator _generator = new();
  internal async Task HandleAsync(string outputPath, CancellationToken cancellationToken)
  {
    var model = new V1FlowSchema()
    {
      ApiVersion = "flowcontrol.apiserver.k8s.io/v1",
      Kind = "FlowSchema",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      Spec = new V1FlowSchemaSpec()
    };
    await _generator.GenerateAsync(model, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
