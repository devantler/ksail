using System.CommandLine;

namespace KSail.Commands;

public class ValidateCommand : Command
{
  public ValidateCommand() : base("validate", "validate manifests in a specified directory")
  {
  }
}
