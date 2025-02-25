using System.ComponentModel;

namespace KSail.Models.Distribution;

/// <summary>
/// Options for the distribution.
/// </summary>
public class KSailDistribution
{
  /// <summary>
  /// Show clusters from all supported distributions.
  /// </summary>
  [Description("Show clusters from all supported distributions.")]
  public bool ShowAllClustersInListings { get; set; }
}
