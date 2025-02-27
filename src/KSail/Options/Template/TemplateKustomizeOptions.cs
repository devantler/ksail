using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Template;



internal class TemplateKustomizeOptions(KSailCluster config)
{

  public readonly TemplateKustomizeRootOption RootOption = new(config) { Arity = ArgumentArity.ZeroOrOne };
}
