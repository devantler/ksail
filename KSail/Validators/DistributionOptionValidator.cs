using System.CommandLine.Parsing;
using KSail.Options;

namespace KSail.Validators;

class DistributionOptionValidator(DistributionOption distributionOption)
{
  internal void Validate(CommandResult symbolResult)
  {
    if (symbolResult.GetValueForOption(distributionOption) is null)
      symbolResult.ErrorMessage = "The distribution option could not be determined from the command line options.";
  }
}
