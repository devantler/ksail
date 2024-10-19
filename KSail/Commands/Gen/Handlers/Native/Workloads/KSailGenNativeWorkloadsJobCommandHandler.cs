using Devantler.KubernetesGenerator.Native.Workloads;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Workloads;

class KSailGenNativeWorkloadsJobCommandHandler
{
  readonly JobGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1Job
    {
      ApiVersion = "batch/v1",
      Kind = "Job",
      Metadata = new V1ObjectMeta
      {
        Name = "<name>"
      },
      Spec = new V1JobSpec
      {
        Template = new V1PodTemplateSpec
        {
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
            ],
            RestartPolicy = "OnFailure"
          }
        }
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
