using System.CommandLine.Invocation;
using k8s;

namespace KSail.Commands.Check.Handlers;

static class KSailCheckHandler
{
  static readonly Kubernetes kubernetesClient = new(KubernetesClientConfiguration.BuildDefaultConfig());
  internal static async Task<Action<InvocationContext>> HandleAsync(string name, string[] kustomizations, CancellationToken cancellationToken)
  {
    foreach (var kustomization in kustomizations)
    {
      var listResponse = await kubernetesClient.ListNamespacedCustomObjectAsync("kustomize.toolkit.fluxcd.io", "v1beta1", "flux-system", "kustomizations", watch: true, cancellationToken: cancellationToken);

      var watcher = listResponse
    }
  }
