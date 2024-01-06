namespace KSail.Provisioners.Registry;

interface IRegistryProvisioner
{
  Task ProvisionAsync(string name, int port, Uri? proxyUrl = null);

  Task DeprovisionAsync(string name);
}
