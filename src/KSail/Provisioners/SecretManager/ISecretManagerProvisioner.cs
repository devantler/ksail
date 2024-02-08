namespace KSail.Provisioners.SecretManager;

interface ISecretManagerProvisioner
{
  Task<int> CreateKeyAsync(KeyType keyType, string keyName, CancellationToken token);
  Task<int> DeleteKeyAsync(KeyType keyType, string keyName, CancellationToken token);
  Task<int> ProvisionAsync(KeyType keyType, string keyName, string k8sContext, CancellationToken token);
  Task<(int exitCode, string result)> GetPublicKeyAsync(KeyType keyType, string keyName, CancellationToken token);
  Task<(int exitCode, string result)> GetPrivateKeyAsync(KeyType keyType, string keyName, CancellationToken token);
  Task<(int exitCode, bool result)> KeyExistsAsync(KeyType keyType, string keyName, CancellationToken token);
}
