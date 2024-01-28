using System.CommandLine;
using System.CommandLine.Binding;

namespace KSail.Commands.Up.Binders;

class KSailUpArgumentsAndOptionsBinder(
  Argument<string> clusterNameArgument,
  Option<string> configOption,
  Option<string> manifestsOption,
  Option<string> kustomizationsOption,
  Option<int> timeoutOption,
  Option<bool> noSOPSOption
) : BinderBase<KSailUpArgumentsAndOptions>
{
  readonly Argument<string> _clusterNameArgument = clusterNameArgument;
  readonly Option<string> _configOption = configOption;
  readonly Option<string> _manifestsOption = manifestsOption;
  readonly Option<string> _kustomizationsOption = kustomizationsOption;
  readonly Option<int> _timeoutOption = timeoutOption;
  readonly Option<bool> _noSOPSOption = noSOPSOption;

  protected override KSailUpArgumentsAndOptions GetBoundValue(BindingContext bindingContext) =>
      new()
      {
        ClusterName = bindingContext.ParseResult.GetValueForArgument(_clusterNameArgument),
        Config = bindingContext.ParseResult.GetValueForOption(_configOption) ?? string.Empty,
        Manifests = bindingContext.ParseResult.GetValueForOption(_manifestsOption) ?? string.Empty,
        Kustomizations = bindingContext.ParseResult.GetValueForOption(_kustomizationsOption) ?? string.Empty,
        Timeout = bindingContext.ParseResult.GetValueForOption(_timeoutOption),
        NoSOPS = bindingContext.ParseResult.GetValueForOption(_noSOPSOption)
      };
}
