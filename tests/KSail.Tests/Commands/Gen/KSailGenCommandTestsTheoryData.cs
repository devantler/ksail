using System.Diagnostics.CodeAnalysis;

namespace KSail.Tests.Commands.Gen;

[ExcludeFromCodeCoverage]
static class KSailGenCommandTestsTheoryData
{
  public static TheoryData<string> HelpTheoryData =>
    new()
    {
      { "gen" },
      { "gen --help" },
      { "gen cert-manager" },
      { "gen cert-manager --help" },
      { "gen config" },
      { "gen config --help" },
      { "gen flux" },
      { "gen flux --help" },
      { "gen kustomize" },
      { "gen kustomize --help" },
      { "gen native" },
      { "gen native --help" }
    };
  public static TheoryData<string, string> GenerateNativeResourceTheoryData =>
    new()
    {
      { "gen native cluster-role-binding", "cluster-role-binding.yaml" },
      { "gen native cluster-role", "cluster-role.yaml" },
      { "gen native namespace", "namespace.yaml" },
      { "gen native network-policy", "network-policy.yaml" },
      { "gen native persistent-volume", "persistent-volume.yaml" },
      { "gen native resource-quota", "resource-quota.yaml" },
      { "gen native role-binding", "role-binding.yaml" },
      { "gen native role", "role.yaml" },
      { "gen native service-account", "service-account.yaml" },
      { "gen native config-map", "config-map.yaml" },
      { "gen native persistent-volume-claim", "persistent-volume-claim.yaml" },
      { "gen native secret", "secret.yaml" },
      { "gen native horizontal-pod-autoscaler", "horizontal-pod-autoscaler.yaml" },
      { "gen native pod-disruption-budget", "pod-disruption-budget.yaml" },
      { "gen native ingress", "ingress.yaml" },
      { "gen native service", "service.yaml" },
      { "gen native cron-job", "cron-job.yaml" },
      { "gen native daemon-set", "daemon-set.yaml" },
      { "gen native deployment", "deployment.yaml" },
      { "gen native job", "job.yaml" },
      { "gen native stateful-set", "stateful-set.yaml" }
    };
  public static TheoryData<string, string> GenerateCertManagerResourceTheoryData =>
    new()
    {
      { "gen cert-manager certificate", "cert-manager-certificate.yaml" },
      { "gen cert-manager cluster-issuer", "cert-manager-cluster-issuer.yaml" }
    };
  public static TheoryData<string, string> GenerateConfigResourceTheoryData =>
    new()
    {
      { "gen config k3d", "k3d-config.yaml" },
      { "gen config ksail", "ksail-config.yaml" },
      { "gen config sops", ".sops.yaml" }
    };
  public static TheoryData<string, string> GenerateFluxResourceTheoryData =>
    new()
    {
      { "gen flux helm-release", "flux-helm-release.yaml" },
      { "gen flux helm-repository", "flux-helm-repository.yaml" },
      { "gen flux kustomization", "flux-kustomization.yaml" }
    };
  public static TheoryData<string, string> GenerateKustomizeResourceTheoryData =>
    new()
    {
      { "gen kustomize component", "kustomize-component.yaml" },
      { "gen kustomize kustomization", "kustomize-kustomization.yaml" }
    };
}
