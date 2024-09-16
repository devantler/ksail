namespace KSail.Tests.Commands.Gen;

static class KSailGenCommandTestsTheoryData
{
  public static TheoryData<string> HelpTheoryData =>
    new()
    {
      { "" },
      { "-h" },
      { "--help" },
      { "native" },
      { "native -h" },
      { "native --help" },
      { "native cluster" },
      { "native cluster -h" },
      { "native cluster --help" },
      { "native config-and-storage"},
      { "native config-and-storage -h" },
      { "native config-and-storage --help" },
      { "native metadata" },
      { "native metadata -h" },
      { "native services" },
      { "native services -h" },
      { "native services --help" },
      { "native workloads" },
      { "native workloads -h" },
      { "native workloads --help" }
    };
  public static TheoryData<string, string> GenerateNativeResourceTheoryData =>
    new()
    {
      { "native cluster api-service", "api-service.yaml" },
      { "native cluster cluster-role-binding", "cluster-role-binding.yaml" },
      { "native cluster cluster-role", "cluster-role.yaml" },
      { "native cluster flow-schema", "flow-schema.yaml" },
      { "native cluster namespace", "namespace.yaml" },
      { "native cluster network-policy", "network-policy.yaml" },
      { "native cluster persistent-volume", "persistent-volume.yaml" },
      { "native cluster priority-level-configuration", "priority-level-configuration.yaml" },
      { "native cluster resource-quota", "resource-quota.yaml" },
      { "native cluster role-binding", "role-binding.yaml" },
      { "native cluster role", "role.yaml" },
      { "native cluster runtime-class", "runtime-class.yaml" },
      { "native cluster service-account", "service-account.yaml" },
      { "native cluster storage-version-migration", "storage-version-migration.yaml" },
      { "native config-and-storage config-map", "config-map.yaml" },
      { "native config-and-storage csi-driver", "csi-driver.yaml" },
      { "native config-and-storage persistent-volume-claim", "persistent-volume-claim.yaml" },
      { "native config-and-storage secret", "secret.yaml" },
      { "native config-and-storage storage-class", "storage-class.yaml" },
      { "native config-and-storage volume-attributes-class", "volume-attributes-class.yaml" },
      { "native metadata cluster-trust-bundle", "cluster-trust-bundle.yaml" },
      { "native metadata custom-resource-definition", "custom-resource-definition.yaml" },
      { "native metadata horizontal-pod-autoscaler", "horizontal-pod-autoscaler.yaml" },
      { "native metadata limit-range", "limit-range.yaml" },
      { "native metadata mutating-webhook-configuration", "mutating-webhook-configuration.yaml" },
      { "native metadata pod-disruption-budget", "pod-disruption-budget.yaml" },
      { "native metadata priority-class", "priority-class.yaml" },
      { "native metadata validating-admission-policy-binding", "validating-admission-policy-binding.yaml" },
      { "native metadata validating-admission-policy", "validating-admission-policy.yaml" },
      { "native metadata validating-webhook-configuration", "validating-webhook-configuration.yaml" },
      { "native services ingress-class", "ingress-class.yaml" },
      { "native services ingress", "ingress.yaml" },
      { "native services service", "service.yaml" },
      { "native workloads cron-job", "cron-job.yaml" },
      { "native workloads daemon-set", "daemon-set.yaml" },
      { "native workloads deployment", "deployment.yaml" },
      { "native workloads job", "job.yaml" },
      { "native workloads replica-set", "replica-set.yaml" },
      { "native workloads stateful-set", "stateful-set.yaml" }
    };
}
