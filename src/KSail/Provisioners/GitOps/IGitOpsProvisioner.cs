namespace KSail.Provisioners.GitOps;

interface IGitOpsProvisioner
{
  Task InstallAsync(string context, string sourceUrl, string path);
  Task UninstallAsync(string context);
  Task ReconcileAsync(string context);
  Task PushManifestsAsync(string context, string ociUrl, string manifestsPath);
}
