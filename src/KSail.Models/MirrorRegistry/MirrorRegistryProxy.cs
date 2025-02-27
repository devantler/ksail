using System.ComponentModel;

namespace KSail.Models.MirrorRegistry;


public class KSailMirrorRegistryProxy
{

  [Description("The URL of the upstream registry to proxy and cache images from.")]
  public required Uri Url { get; set; }


  [Description("The username to authenticate with the upstream registry.")]
  public string? Username { get; set; }


  [Description("The password to authenticate with the upstream registry.")]
  public string? Password { get; set; }


  [Description("Connect to the upstream registry over HTTPS.")]
  public bool Insecure { get; set; }
}
