namespace KSail.Tests.Commands.Gen;

sealed class TheoryData
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
      { "native service" },
      { "native service -h" },
      { "native service --help" },
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
      { "native cluster storage-version-migration", "storage-version-migration.yaml" }
    };
}
