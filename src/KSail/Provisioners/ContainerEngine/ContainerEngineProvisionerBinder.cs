using System.CommandLine.Binding;

namespace KSail.Provisioners.ContainerEngine;

class ContainerEngineProvisionerBinder(Enums.ContainerEngineType containerEngine) : BinderBase<IContainerEngineProvisioner>
{
  readonly Enums.ContainerEngineType _containerEngine = containerEngine;

  protected override IContainerEngineProvisioner GetBoundValue(
      BindingContext bindingContext)
  {
    return _containerEngine switch
    {
      Enums.ContainerEngineType.Docker => new DockerProvisioner(),
      _ => throw new NotSupportedException($"🚨 Container engine type '{_containerEngine}' is not supported."),
    };
  }
}
