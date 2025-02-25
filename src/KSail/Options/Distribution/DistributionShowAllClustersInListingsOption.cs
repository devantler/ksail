using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Distribution;

/// <summary>
/// Show all clusters in listings.
/// </summary>
/// <param name="config"></param>
public class DistributionShowAllClustersInListingsOption(KSailCluster config) : Option<bool?>(
  ["--all", "-a"],
  $"List clusters from all distributions. [default: {config.Spec.Distribution.ShowAllClustersInListings}]"
);
