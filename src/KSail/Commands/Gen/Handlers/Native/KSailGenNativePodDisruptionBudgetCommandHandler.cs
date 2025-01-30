using Devantler.KubernetesGenerator.Native;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativePodDisruptionBudgetCommandHandler
{
  readonly PodDisruptionBudgetGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1PodDisruptionBudget()
    {
      ApiVersion = "policy/v1",
      Kind = "PodDisruptionBudget",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>"
      },
      Spec = new V1PodDisruptionBudgetSpec()
      {
        Selector = new V1LabelSelector()
        {
          MatchLabels = new Dictionary<string, string>()
        },
        MinAvailable = new IntstrIntOrString("2"),
        MaxUnavailable = new IntstrIntOrString("1"),
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
