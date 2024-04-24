using KSail.Enums;
using KSail.Models.KSail.Options;

namespace KSail.Models.KSail;

class KSailConfig(string clusterName)
{
  public string Name { get; set; } = clusterName;
  public ContainerOrchestratorType ContainerOrchestrator { get; set; } = ContainerOrchestratorType.Kubernetes;
  public ContainerEngineType ContainerEngine { get; set; } = ContainerEngineType.Docker;
  public KubernetesOptions Kubernetes { get; set; } = new();
  public RegistryOptions Registries { get; set; } = new();
  public GitOpsOptions GitOps { get; set; } = new();
  public LintingOptions Linting { get; set; } = new();
  public ValidationOptions Validation { get; set; } = new();
  public SOPSOptions SOPS { get; set; } = new();
}
