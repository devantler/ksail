using System.CommandLine.Binding;
using KSail.Exceptions;

namespace KSail.Provisioners.GitOps;

class GitOpsProvisionerBinder(Enums.GitOpsType gitOps) : BinderBase<IGitOpsProvisioner>
{
  readonly Enums.GitOpsType _gitOps = gitOps;

  protected override IGitOpsProvisioner GetBoundValue(
      BindingContext bindingContext)
  {
    return _gitOps switch
    {
      Enums.GitOpsType.Flux => new FluxProvisioner(),
      _ => throw new KSailException($"🚨 Unknown container engine: {_gitOps}"),
    };
  }
}
