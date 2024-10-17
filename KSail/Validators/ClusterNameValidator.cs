using System.CommandLine.Parsing;
using Devantler.KubernetesGenerator.KSail.Models;
using KSail.Options;

namespace KSail.Validators;

class ClusterNameOptionValidator(KSailCluster config, NameOption clusterNameOption)
{
  internal void Validate(CommandResult symbolResult)
  {
    if (string.IsNullOrEmpty(symbolResult.GetValueForOption(clusterNameOption)) && (string.IsNullOrEmpty(config.Metadata?.Name) || config.Metadata?.Name == "default"))
      symbolResult.ErrorMessage = "The cluster name option is required.";
  }
}
