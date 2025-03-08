using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Generator;


class GeneratorOptions(KSailCluster config)
{
  public readonly GeneratorOverwriteOption OverwriteOption = new(config) { Arity = ArgumentArity.ZeroOrOne };
}
