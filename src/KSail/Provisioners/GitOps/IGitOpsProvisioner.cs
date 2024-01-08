namespace KSail.Provisioners.GitOps;

interface IGitOpsProvisioner : IProvisioner
{
  Task CheckPrerequisitesAsync();

  Task InstallAsync(string sourceUrl, string fluxKustomizationPathOption);

  Task UninstallAsync();

  Task PushManifestsAsync(string ociUrl, string manifestsPath);
}
