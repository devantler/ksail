namespace KSail.Models;

internal sealed class K3dConfig
{
  public K3dConfigMetadata Metadata { get; set; } = new();
}

internal sealed class K3dConfigMetadata
{
  public string? Name { get; set; }
}
