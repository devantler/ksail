namespace KSail.Models;

sealed record K3dConfig(K3dConfigMetadata Metadata);

sealed record K3dConfigMetadata(string Name);
