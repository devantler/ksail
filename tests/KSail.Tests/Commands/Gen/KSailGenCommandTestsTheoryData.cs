using System.Diagnostics.CodeAnalysis;

namespace KSail.Tests.Commands.Gen;

[ExcludeFromCodeCoverage]
static class KSailGenCommandTestsTheoryData
{
  public static TheoryData<string> HelpTheoryData =>
    new()
    {
      { "" },
      { "-h" },
      { "--help" },
      { "cert-manager" },
      { "cert-manager -h" },
      { "cert-manager --help" },
      { "config" },
      { "config -h" },
      { "config --help" },
      { "flux" },
      { "flux -h" },
      { "flux --help" },
      { "kustomize" },
      { "kustomize -h" },
      { "kustomize --help" },
      { "native" },
      { "native -h" },
      { "native --help" }
    };
  public static TheoryData<string, string> GenerateNativeResourceTheoryData =>
    new()
    {
      { "native cluster-role-binding", "cluster-role-binding.yaml" },
      { "native cluster-role", "cluster-role.yaml" },
      { "native namespace", "namespace.yaml" },
      { "native network-policy", "network-policy.yaml" },
      { "native persistent-volume", "persistent-volume.yaml" },
      { "native resource-quota", "resource-quota.yaml" },
      { "native role-binding", "role-binding.yaml" },
      { "native role", "role.yaml" },
      { "native service-account", "service-account.yaml" },
      { "native config-map", "config-map.yaml" },
      { "native persistent-volume-claim", "persistent-volume-claim.yaml" },
      { "native secret", "secret.yaml" },
      { "native horizontal-pod-autoscaler", "horizontal-pod-autoscaler.yaml" },
      { "native pod-disruption-budget", "pod-disruption-budget.yaml" },
      { "native ingress", "ingress.yaml" },
      { "native service", "service.yaml" },
      { "native cron-job", "cron-job.yaml" },
      { "native daemon-set", "daemon-set.yaml" },
      { "native deployment", "deployment.yaml" },
      { "native job", "job.yaml" },
      { "native stateful-set", "stateful-set.yaml" }
    };
  public static TheoryData<string, string> GenerateCertManagerResourceTheoryData =>
    new()
    {
      { "cert-manager certificate", "cert-manager-certificate.yaml" },
      { "cert-manager cluster-issuer", "cert-manager-cluster-issuer.yaml" }
    };
  public static TheoryData<string, string> GenerateConfigResourceTheoryData =>
    new()
    {
      { "config k3d", "k3d-config.yaml" },
      { "config ksail", "ksail-config.yaml" },
      { "config sops", ".sops.yaml" }
    };
  public static TheoryData<string, string> GenerateFluxResourceTheoryData =>
    new()
    {
      { "flux helm-release", "flux-helm-release.yaml" },
      { "flux helm-repository", "flux-helm-repository.yaml" },
      { "flux kustomization", "flux-kustomization.yaml" }
    };
  public static TheoryData<string, string> GenerateKustomizeResourceTheoryData =>
    new()
    {
      { "kustomize component", "kustomize-component.yaml" },
      { "kustomize kustomization", "kustomize-kustomization.yaml" }
    };
}
