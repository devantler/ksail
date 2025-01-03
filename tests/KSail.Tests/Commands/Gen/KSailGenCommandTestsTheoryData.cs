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
      { "cert-manager certificate", "cert-manager-certificate.yaml" },
      { "cert-manager cluster-issuer", "cert-manager-cluster-issuer.yaml" },
      { "config k3d", "k3d-config.yaml" },
      { "config ksail", "ksail-config.yaml" },
      { "config sops", ".sops.yaml" },
      { "flux helm-release", "flux-helm-release.yaml" },
      { "flux helm-repository", "flux-helm-repository.yaml" },
      { "flux kustomization", "flux-kustomization.yaml" },
      { "kustomize component", "kustomize-component.yaml" },
      { "kustomize kustomization", "kustomize-kustomization.yaml" },
      { "native api-service", "api-service.yaml" },
      { "native cluster-role-binding", "cluster-role-binding.yaml" },
      { "native cluster-role", "cluster-role.yaml" },
      { "native flow-schema", "flow-schema.yaml" },
      { "native namespace", "namespace.yaml" },
      { "native network-policy", "network-policy.yaml" },
      { "native persistent-volume", "persistent-volume.yaml" },
      { "native priority-level-configuration", "priority-level-configuration.yaml" },
      { "native resource-quota", "resource-quota.yaml" },
      { "native role-binding", "role-binding.yaml" },
      { "native role", "role.yaml" },
      { "native runtime-class", "runtime-class.yaml" },
      { "native service-account", "service-account.yaml" },
      { "native storage-version-migration", "storage-version-migration.yaml" },
      { "native config-map", "config-map.yaml" },
      { "native csi-driver", "csi-driver.yaml" },
      { "native persistent-volume-claim", "persistent-volume-claim.yaml" },
      { "native secret", "secret.yaml" },
      { "native storage-class", "storage-class.yaml" },
      { "native volume-attributes-class", "volume-attributes-class.yaml" },
      { "native cluster-trust-bundle", "cluster-trust-bundle.yaml" },
      { "native custom-resource-definition", "custom-resource-definition.yaml" },
      { "native horizontal-pod-autoscaler", "horizontal-pod-autoscaler.yaml" },
      { "native limit-range", "limit-range.yaml" },
      { "native mutating-webhook-configuration", "mutating-webhook-configuration.yaml" },
      { "native pod-disruption-budget", "pod-disruption-budget.yaml" },
      { "native priority-class", "priority-class.yaml" },
      { "native validating-admission-policy-binding", "validating-admission-policy-binding.yaml" },
      { "native validating-admission-policy", "validating-admission-policy.yaml" },
      { "native validating-webhook-configuration", "validating-webhook-configuration.yaml" },
      { "native ingress-class", "ingress-class.yaml" },
      { "native ingress", "ingress.yaml" },
      { "native service", "service.yaml" },
      { "native cron-job", "cron-job.yaml" },
      { "native daemon-set", "daemon-set.yaml" },
      { "native deployment", "deployment.yaml" },
      { "native job", "job.yaml" },
      { "native replica-set", "replica-set.yaml" },
      { "native stateful-set", "stateful-set.yaml" }
    };
}
