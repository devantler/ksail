using System.CommandLine.Parsing;
using Devantler.KubernetesGenerator.KSail.Models;
using KSail.Options;

namespace KSail.Validators;

class DistributionOptionValidator(KSailCluster config, DistributionOption distributionOption)
{
  internal void Validate(CommandResult symbolResult)
  {
    if (symbolResult.GetValueForOption(distributionOption) is null && config.Spec?.Distribution is null)
      symbolResult.ErrorMessage = "The distribution option is required.";
  }
}
