using System.CommandLine.Binding;
using KSail.Exceptions;

namespace KSail.Services.Provisioners.ContainerEngine;

class ContainerEngineProvisionerBinder(Enums.ContainerEngineType containerEngine) : BinderBase<IContainerEngineProvisioner>
{
  readonly Enums.ContainerEngineType _containerEngine = containerEngine;

  protected override IContainerEngineProvisioner GetBoundValue(
      BindingContext bindingContext)
  {
    return _containerEngine switch
    {
      Enums.ContainerEngineType.Docker => new DockerProvisioner(),
      _ => throw new KSailException($"🚨 Unknown container engine: {_containerEngine}"),
    };
  }
}
