using System.CommandLine.Binding;

namespace KSail.Provisioners.ContainerEngine;

class ContainerEngineProvisionerBinder : BinderBase<IContainerEngineProvisioner>
{
  protected override IContainerEngineProvisioner GetBoundValue(
      BindingContext bindingContext) => new DockerProvisioner();
}
