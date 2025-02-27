using System.ComponentModel;

namespace KSail.Models.LocalRegistry;


public class KSailLocalRegistry
{

  [Description("The name of the registry.")]
  public required string Name { get; set; }


  [Description("The host port of the registry (if applicable).")]
  public int HostPort { get; set; }


  [Description("The username to authenticate with the registry.")]
  public string? Username { get; set; }


  [Description("The password to authenticate with the registry.")]
  public string? Password { get; set; }


  [Description("The registry provider.")]
  public KSailLocalRegistryProvider Provider { get; set; } = KSailLocalRegistryProvider.Docker;
}
