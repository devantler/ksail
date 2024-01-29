namespace KSail.Provisioners.GitOps;

interface IGitOpsProvisioner
{
  Task<int> InstallAsync(string context, string sourceUrl, string path, CancellationToken token);
  Task<int> ReconcileAsync(string context, CancellationToken token);
  Task<int> PushManifestsAsync(string ociUrl, string manifestsPath, CancellationToken token);
}
