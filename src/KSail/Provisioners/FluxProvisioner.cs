using KSail.CLIWrappers;

namespace KSail.Provisioners;

sealed class FluxProvisioner : IProvisioner
{
  internal static async Task CheckPrerequisitesAsync()
  {
    console.WriteLine("🔄 Checking Flux prerequisites are satisfied...");
    await FluxCLIWrapper.CheckPrerequisitesAsync();
    console.WriteLine();
  }

  internal static async Task InstallAsync(string sourceUrl, string fluxKustomizationPathOption)
  {
    console.WriteLine("🔄 Installing Flux...");
    await FluxCLIWrapper.InstallAsync();
    console.WriteLine();
    console.WriteLine("🔄 Creating Flux OCI source...");
    await FluxCLIWrapper.CreateSourceOCIAsync(sourceUrl);
    console.WriteLine();
    console.WriteLine("🔄 Creating Flux kustomization...");
    await FluxCLIWrapper.CreateKustomizationAsync(fluxKustomizationPathOption);
    console.WriteLine();
  }

  internal static async Task UninstallAsync()
  {
    console.WriteLine("🔄 Uninstalling Flux...");
    await FluxCLIWrapper.UninstallAsync();
    console.WriteLine();
  }

  internal static async Task PushManifestsAsync(string ociUrl, string manifestsPath) =>
    await FluxCLIWrapper.PushManifestsAsync(ociUrl, manifestsPath);
}
