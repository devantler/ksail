
using KSail.CLIWrappers;

namespace KSail.Provisioners.GitOps;

sealed class FluxProvisioner : IGitOpsProvisioner
{
  public async Task CheckPrerequisitesAsync()
  {
    Console.WriteLine("🔄 Checking Flux prerequisites are satisfied...");
    await FluxCLIWrapper.CheckPrerequisitesAsync();
    Console.WriteLine();
  }

  public async Task InstallAsync(string sourceUrl, string fluxKustomizationPathOption)
  {
    Console.WriteLine("🔄 Installing Flux...");
    await FluxCLIWrapper.InstallAsync();
    Console.WriteLine();
    Console.WriteLine("🔄 Creating Flux OCI source...");
    await FluxCLIWrapper.CreateSourceOCIAsync(sourceUrl);
    Console.WriteLine();
    Console.WriteLine("🔄 Creating Flux kustomization...");
    await FluxCLIWrapper.CreateKustomizationAsync(fluxKustomizationPathOption);
    Console.WriteLine();
  }

  public async Task UninstallAsync()
  {
    Console.WriteLine("🔄 Uninstalling Flux...");
    await FluxCLIWrapper.UninstallAsync();
    Console.WriteLine();
  }

  public async Task PushManifestsAsync(string ociUrl, string manifestsPath)
  {
    Console.WriteLine($"📥 Pushing manifests to {ociUrl}...");
    await FluxCLIWrapper.PushManifestsAsync(ociUrl, manifestsPath);
    Console.WriteLine();
  }
}
