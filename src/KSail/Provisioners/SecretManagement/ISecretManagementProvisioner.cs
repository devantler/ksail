namespace KSail.Provisioners.SecretManagement;

interface ISecretManagementProvisioner : IProvisioner
{
  Task CreateKeysAsync();

  Task DeploySecretManagementAsync();
}
