using System.CommandLine.Binding;
using KSail.Exceptions;

namespace KSail.Provisioners.ContainerEngine;

class ContainerEngineProvisionerBinder(ContainerEngine containerEngine) : BinderBase<IContainerEngineProvisioner>
{
  readonly ContainerEngine containerEngine = containerEngine;

  protected override IContainerEngineProvisioner GetBoundValue(
      BindingContext bindingContext)
  {
    return containerEngine switch
    {
      ContainerEngine.Docker => new DockerProvisioner(),
      _ => throw new KSailException($"🚨 Unknown container engine: {containerEngine}"),
    };
  }
}
