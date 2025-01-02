using Devantler.KubernetesGenerator.Native.Workloads;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativeWorkloadsReplicaSetCommandHandler
{
  readonly ReplicaSetGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1ReplicaSet
    {
      ApiVersion = "apps/v1",
      Kind = "ReplicaSet",
      Metadata = new V1ObjectMeta
      {
        Name = "<name>"
      },
      Spec = new V1ReplicaSetSpec
      {
        Selector = new V1LabelSelector
        {
          MatchLabels = new Dictionary<string, string>
          {
            ["app"] = "<name>"
          }
        },
        Template = new V1PodTemplateSpec
        {
          Metadata = new V1ObjectMeta
          {
            Labels = new Dictionary<string, string>
            {
              ["app"] = "<name>"
            }
          },
          Spec = new V1PodSpec
          {
            Containers =
            [
              new V1Container
              {
                Name = "<name>",
                Image = "<image>",
                ImagePullPolicy = "IfNotPresent",
                Command = []
              }
            ]
          }
        }
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
