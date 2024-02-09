using KSail.CLIWrappers;
using KSail.Provisioners.ContainerOrchestrator;

namespace KSail.Provisioners.SecretManager;

sealed class LocalSOPSProvisioner() : ISecretManagerProvisioner, IDisposable
{
  readonly KubernetesProvisioner _kubernetesProvisioner = new();

  public async Task<int> ProvisionAsync(KeyType keyType, string keyName, string k8sContext, CancellationToken token)
  {
    switch (keyType)
    {
      case KeyType.Age:
        var (exitCode, result) = await GetPrivateKeyAsync(KeyType.Age, keyName, token);
        if (exitCode != 0)
        {
          return 1;
        }
        await _kubernetesProvisioner.CreateSecretAsync(k8sContext, "sops-age", new Dictionary<string, string>
        {
          ["sops.agekey"] = result
        }, "flux-system");
        return 0;
      default:
        throw new NotSupportedException($"🚨 Unsupported key type '{keyType}'");
    }
  }

  public Task<int> CreateKeyAsync(KeyType keyType, string keyName, CancellationToken token)
  {
    return keyType switch
    {
      KeyType.Age => AgeCLIWrapper.GenerateKeyAsync(keyName, true, token),
      _ => throw new NotSupportedException($"🚨 Unsupported key type '{keyType}'"),
    };
  }

  public async Task<(int exitCode, string result)> GetPublicKeyAsync(KeyType keyType, string keyName, CancellationToken token)
  {
    switch (keyType)
    {
      case KeyType.Age:
        string publicKeyLine = (await File.ReadAllLinesAsync($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/age/{keyName}.agekey", token)).Skip(1).First();
        int startIndex = publicKeyLine.IndexOf("age", StringComparison.Ordinal);
        return (0, publicKeyLine[startIndex..]);
      default:
        throw new NotSupportedException($"🚨 Unsupported key type '{keyType}'");
    }
  }

  public async Task<(int exitCode, string result)> GetPrivateKeyAsync(KeyType keyType, string keyName, CancellationToken token)
  {
    switch (keyType)
    {
      case KeyType.Age:
        string privateKey = (await File.ReadAllLinesAsync($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/age/{keyName}.agekey", token)).Last();
        return (0, privateKey);
      default:
        throw new NotSupportedException($"🚨 Unsupported key type '{keyType}'");
    }
  }

  public Task<(int exitCode, bool result)> KeyExistsAsync(KeyType keyType, string keyName, CancellationToken token)
  {
    return keyType switch
    {
      KeyType.Age => Task.FromResult((0, File.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/age/{keyName}.agekey"))),
      _ => throw new NotSupportedException($"🚨 Unsupported key type '{keyType}'"),
    };
  }

  public Task<int> DeleteKeyAsync(KeyType keyType, string keyName, CancellationToken token)
  {
    switch (keyType)
    {
      case KeyType.Age:
        if (File.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/age/{keyName}.agekey"))
        {
          File.Delete($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/age/{keyName}.agekey");
        }
        return Task.FromResult(0);
      default:
        throw new NotSupportedException($"🚨 Unsupported key type '{keyType}'");
    }
  }

  public void Dispose()
  {
    _kubernetesProvisioner.Dispose();
    GC.SuppressFinalize(this);
  }
}
