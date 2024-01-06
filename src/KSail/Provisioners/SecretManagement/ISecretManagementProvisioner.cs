namespace KSail.Provisioners.SecretManagement;

/// <summary>
/// The interface for a secret management provisioner.
/// </summary>
public interface ISecretManagementProvisioner : IProvisioner
{
  /// <summary>
  /// Creates keys needed for encrypting and decrypting secrets.
  /// </summary>
  Task CreateKeysAsync();

  /// <summary>
  /// Deploys the secret management agent.
  /// </summary>
  Task DeploySecretManagementAsync();
}
