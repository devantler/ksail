using System.CommandLine.Binding;

namespace KSail.Provisioners.KubernetesDistribution;

class KubernetesDistributionProvisionerBinder(Enums.KubernetesDistributionType kubernetesDistributionType) : BinderBase<IKubernetesDistributionProvisioner>
{
  readonly Enums.KubernetesDistributionType _kubernetesDistributionType = kubernetesDistributionType;

  protected override IKubernetesDistributionProvisioner GetBoundValue(
      BindingContext bindingContext)
  {
    return _kubernetesDistributionType switch
    {
      Enums.KubernetesDistributionType.K3d => new K3dProvisioner(),
      _ => throw new NotSupportedException($"🚨 Kubernetes distribution '{_kubernetesDistributionType}' is not supported."),
    };
  }
}
