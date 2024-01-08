namespace KSail.Models.K3d;

sealed record K3dConfig(K3dConfigMetadata Metadata);

sealed record K3dConfigMetadata(string Name);
