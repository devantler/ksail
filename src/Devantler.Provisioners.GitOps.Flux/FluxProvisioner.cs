using KSail.CLIWrappers;

namespace KSail.Provisioners.GitOps;

sealed class FluxProvisioner : IGitOpsProvisioner
{
  public async Task<int> InstallAsync(string context, string sourceUrl, string path, CancellationToken token)
  {
    return await FluxCLIWrapper.CheckPrerequisitesAsync(context, token) != 0 ||
      await FluxCLIWrapper.InstallAsync(context, token) != 0 ||
      await FluxCLIWrapper.CreateSourceOCIAsync(context, sourceUrl, token) != 0 ||
      await FluxCLIWrapper.CreateKustomizationAsync(context, path, token) != 0
      ? 1
      : 0;
  }

  public async Task<int> ReconcileAsync(string context, CancellationToken token) =>
    await FluxCLIWrapper.ReconcileAsync(context, token) != 0 ? 1 : 0;
  public async Task<int> PushManifestsAsync(string ociUrl, string manifestsPath, CancellationToken token) =>
    await FluxCLIWrapper.PushManifestsAsync(ociUrl, manifestsPath, token) != 0 ? 1 : 0;
}
