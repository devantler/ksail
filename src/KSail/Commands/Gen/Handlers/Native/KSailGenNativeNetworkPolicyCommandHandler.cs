using Devantler.KubernetesGenerator.Native.Cluster;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativeNetworkPolicyCommandHandler
{
  readonly NetworkPolicyGenerator _generator = new();

  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1NetworkPolicy()
    {
      ApiVersion = "networking.k8s.io/v1",
      Kind = "NetworkPolicy",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
        NamespaceProperty = "<namespace>",
      },
      Spec = new V1NetworkPolicySpec()
      {
        PodSelector = new V1LabelSelector()
        {
          MatchLabels = new Dictionary<string, string>()
        },
        PolicyTypes =
        [
          "Ingress",
          "Egress",
        ],
        Ingress = [
          new V1NetworkPolicyIngressRule()
          {
             FromProperty = []
          }
        ],
        Egress = [
          new V1NetworkPolicyEgressRule()
          {
            To = []
          }
        ]
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}


