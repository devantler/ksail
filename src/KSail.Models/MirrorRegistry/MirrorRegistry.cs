using System.ComponentModel;
using KSail.Models.LocalRegistry;

namespace KSail.Models.MirrorRegistry;


public class KSailMirrorRegistry : KSailLocalRegistry
{

  [Description("A proxy for the registry to use to proxy and cache images.")]
  public required KSailMirrorRegistryProxy Proxy { get; set; }
}
