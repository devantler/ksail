using System.ComponentModel;
using KSail.Models.Project.Enums;

namespace KSail.Models;


public class KSailCluster
{

  [Description("The API version where the KSail Cluster object is defined.")]
  public string ApiVersion { get; set; } = "ksail.io/v1alpha1";

  [Description("The KSail Cluster object kind.")]
  public string Kind { get; set; } = "Cluster";

  [Description("The metadata of the KSail Cluster object.")]
  public KSailMetadata Metadata { get; set; } = new();

  [Description("The spec of the KSail Cluster object.")]
  public KSailClusterSpec Spec { get; set; } = new();


  public KSailCluster() =>
    Spec = new KSailClusterSpec(Metadata.Name);



  public KSailCluster(string name)
  {
    Metadata.Name = name;
    Spec = new KSailClusterSpec(name);
  }



  public KSailCluster(KSailKubernetesDistributionType distribution) =>
    Spec = new KSailClusterSpec(Metadata.Name, distribution);




  public KSailCluster(string name, KSailKubernetesDistributionType distribution)
  {
    Metadata.Name = name;
    Spec = new KSailClusterSpec(name, distribution);
  }
}
