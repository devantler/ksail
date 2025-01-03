using KSail.Models.Registry;

namespace KSail.Models.MirrorRegistry;

/// <summary>
/// The options for mirror registries.
/// </summary>
public class KSailMirrorRegistryOptions
{
  /// <summary>
  /// The mirror registries to create for the KSail cluster.
  /// </summary>
  public IEnumerable<KSailMirrorRegistry> MirrorRegistries { get; set; } = [
    new KSailMirrorRegistry { Name = "registry.k8s.io", HostPort = 5556, Proxy = new KSailRegistryProxy { Url = new Uri("https://registry.k8s.io") } },
    new KSailMirrorRegistry { Name = "docker.io", HostPort = 5557,  Proxy = new KSailRegistryProxy { Url = new Uri("https://registry-1.docker.io") } },
    new KSailMirrorRegistry { Name = "ghcr.io", HostPort = 5558, Proxy = new KSailRegistryProxy { Url = new Uri("https://ghcr.io") } },
    new KSailMirrorRegistry { Name = "gcr.io", HostPort = 5559, Proxy = new KSailRegistryProxy { Url = new Uri("https://gcr.io") } },
    new KSailMirrorRegistry { Name = "mcr.microsoft.com", HostPort = 5560, Proxy = new KSailRegistryProxy { Url = new Uri("https://mcr.microsoft.com") } },
    new KSailMirrorRegistry { Name = "quay.io", HostPort = 5561, Proxy = new KSailRegistryProxy { Url = new Uri("https://quay.io") } },
  ];
}
