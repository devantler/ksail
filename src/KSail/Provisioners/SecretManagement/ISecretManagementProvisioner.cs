namespace KSail.Provisioners.SecretManagement;

interface ISecretManagementProvisioner : IProvisioner
{
  Task CreateKeysAsync();

  Task ProvisionAsync();

  Task ShowPublicKeyAsync();

  Task ShowPrivateKeyAsync();
}
