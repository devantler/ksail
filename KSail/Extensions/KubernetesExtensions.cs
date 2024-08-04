using k8s;
using k8s.Autorest;

namespace KSail.Extensions;

static class KubernetesExtensions
{
  internal static Task<HttpOperationResponse<object>> ListKustomizationsWithHttpMessagesAsync(this Kubernetes kubernetesClient)
  {
    return kubernetesClient.CustomObjects.ListNamespacedCustomObjectWithHttpMessagesAsync(
      "kustomize.toolkit.fluxcd.io",
      "v1",
      "flux-system",
      "kustomizations",
      watch: true,
      cancellationToken: new CancellationToken()
    );
  }
}
