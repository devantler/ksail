namespace KSail.Models;
sealed class K3dConfig
{
  public K3dConfigMetadata Metadata { get; set; } = new();
}

sealed class K3dConfigMetadata
{
  public string? Name { get; set; }
}
