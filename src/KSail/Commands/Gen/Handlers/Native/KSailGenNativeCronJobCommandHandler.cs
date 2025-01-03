using Devantler.KubernetesGenerator.Native.Workloads;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativeWorkloadsCronJobCommandHandler
{
  readonly CronJobGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1CronJob
    {
      ApiVersion = "batch/v1",
      Kind = "CronJob",
      Metadata = new V1ObjectMeta
      {
        Name = "<name>"
      },
      Spec = new V1CronJobSpec
      {
        Schedule = "<schedule>",
        JobTemplate = new V1JobTemplateSpec
        {
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
        }
      }

    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
