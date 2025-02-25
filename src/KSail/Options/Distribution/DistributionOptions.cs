using KSail.Models;

namespace KSail.Options.Distribution;

/// <summary>
/// Options for the distribution.
/// </summary>
/// <param name="config"></param>
public class DistributionOptions(KSailCluster config)
{
  /// <summary>
  /// Show all clusters in listings.
  /// </summary>
  public DistributionShowAllClustersInListingsOption ShowAllClustersInListings { get; set; } = new(config);
}
