namespace KSail.Provisioners.Registry;

interface IRegistryProvisioner : IProvisioner
{
  Task CreateRegistryAsync(string name, int port, Uri? proxyUrl = null);

  Task DeleteRegistryAsync(string name);
}
