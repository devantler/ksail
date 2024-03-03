using KSail.Commands.Init.Generators;
using KSail.Models.Kubernetes.FluxKustomization;
using KSail.Provisioners.SecretManager;

namespace KSail.Commands.Init.Handlers;

class KSailInitCommandHandler(string clusterName, string manifestsDirectory) : IDisposable
{
  readonly LocalSOPSProvisioner _sopsGenerator = new();

  readonly InitFilesGenerator _initFilesGenerator = new();

  internal async Task<int> HandleAsync(CancellationToken token)
  {
    Console.WriteLine($"📁 Initializing new cluster '{clusterName}'");
    if (await ProvisionSOPSKey(clusterName, token) != 0)
    {
      return 1;
    }
    await _initFilesGenerator.GenerateInitFiles(clusterName, manifestsDirectory, token);
    Console.WriteLine("");
    return 0;
  }

  async Task<int> ProvisionSOPSKey(string clusterName, CancellationToken token)
  {
    var (keyExistsExitCode, keyExists) = await _sopsGenerator.KeyExistsAsync(KeyType.Age, clusterName, token);
    if (keyExistsExitCode != 0)
    {
      Console.WriteLine("✕ Unexpected error occurred while checking for an existing Age key for SOPS.");
      return 1;
    }
    Console.WriteLine("✚ Generating SOPS");
    if (!keyExists && await _sopsGenerator.CreateKeyAsync(KeyType.Age, clusterName, token) != 0)
    {
      Console.WriteLine("✕ Unexpected error occurred while creating a new Age key for SOPS.");
      return 1;
    }
    return 0;
  }

  public void Dispose()
  {
    _sopsGenerator.Dispose();
    GC.SuppressFinalize(this);
  }
}
