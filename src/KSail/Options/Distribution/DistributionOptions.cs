using KSail.Models;

namespace KSail.Options.Distribution;



internal class DistributionOptions(KSailCluster config)
{

  public DistributionShowAllClustersInListingsOption ShowAllClustersInListings { get; set; } = new(config);
}
