using System.CommandLine;

namespace KSail.Commands.Check.Options;

internal class KustomizationsOption : Option<string[]>
{
  public KustomizationsOption() : base(["--kustomizations", "-k"], "The kustomizations to check.") {
  
  }
}
