using System.CommandLine;
using KSail.Commands.Init.Models;

namespace KSail.Commands.Init.Options;

sealed class DistributionOption()
 : Option<Distribution>(
    ["-d", "--distribution"],
    () => Distribution.K3d,
    "The distribution to use for the cluster."
  )
{
}
