using System.CommandLine.Binding;

namespace KSail.Provisioners.ContainerOrchestrator;

class ContainerOrchestratorProvisionerBinder(Enums.ContainerOrchestratorType containerOrchestrator) : BinderBase<IContainerOrchestratorProvisioner>
{
  readonly Enums.ContainerOrchestratorType _containerOrchestrator = containerOrchestrator;
  protected override IContainerOrchestratorProvisioner GetBoundValue(
      BindingContext bindingContext)
  {
    return _containerOrchestrator switch
    {
      Enums.ContainerOrchestratorType.Kubernetes => new KubernetesProvisioner(),
      _ => throw new NotSupportedException($"🚨 Container orchestrator type '{_containerOrchestrator}' is not supported."),
    };
  }
}
