using System.CommandLine.Parsing;
using KSail.Options;

namespace KSail.Validators;

class NameOptionValidator(NameOption nameOption)
{
  internal void Validate(CommandResult symbolResult)
  {
    if (string.IsNullOrEmpty(symbolResult.GetValueForOption(nameOption)))
      symbolResult.ErrorMessage = "The cluster name option is required.";
  }
}
