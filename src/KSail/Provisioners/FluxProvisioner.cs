using KSail.CLIWrappers;

namespace KSail.Provisioners;

sealed class FluxProvisioner : IProvisioner
{
  internal static async Task CheckPrerequisitesAsync()
  {
    Console.WriteLine("🔄 Checking Flux prerequisites are satisfied...");
    await FluxCLIWrapper.CheckPrerequisitesAsync();
    Console.WriteLine();
  }

  internal static async Task InstallAsync(string sourceUrl, string fluxKustomizationPathOption)
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

  internal static async Task UninstallAsync()
  {
    Console.WriteLine("🔄 Uninstalling Flux...");
    await FluxCLIWrapper.UninstallAsync();
    Console.WriteLine();
  }

  internal static async Task PushManifestsAsync(string ociUrl, string manifestsPath) =>
    await FluxCLIWrapper.PushManifestsAsync(ociUrl, manifestsPath);
}
