using System.ComponentModel;

namespace KSail.Models.DeploymentTool;


/// <remarks>
/// Constructs a new instance of the KSail repository with the specified URL.
/// </remarks>
public class KSailRepository
{

  [Description("The URL of the repository.")]
  public Uri Url { get; set; } = new Uri("oci://host.docker.internal:5555/ksail-registry");


  public KSailRepository()
  {
  }



  public KSailRepository(Uri url) => Url = url;
}
