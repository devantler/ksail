using System.CommandLine;
using KSail.Models;

namespace KSail.Options.DeploymentTool;


internal class DeploymentToolFluxOptions(KSailCluster config)
{

  public readonly DeploymentToolFluxSourceUrlOption SourceOption = new(config) { Arity = ArgumentArity.ZeroOrOne };
}
