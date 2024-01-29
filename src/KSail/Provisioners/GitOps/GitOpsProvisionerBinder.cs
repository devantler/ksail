using System.CommandLine.Binding;

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
      _ => throw new NotSupportedException($"🚨 GitOps Engine {_gitOps} is not supported."),
    };
  }
}
