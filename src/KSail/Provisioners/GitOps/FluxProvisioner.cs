
using KSail.CLIWrappers;

namespace KSail.Provisioners.GitOps;

sealed class FluxProvisioner : IGitOpsProvisioner
{
  public async Task CheckPrerequisitesAsync()
  {
    Console.WriteLine("🔄 Checking Flux prerequisites are satisfied...");
    await FluxCLIWrapper.CheckPrerequisitesAsync();
    Console.WriteLine("🔄✅ Flux prerequisites are satisfied...");
  }

  public async Task InstallAsync(string sourceUrl, string fluxKustomizationPathOption)
  {
    Console.WriteLine("🔄 Installing Flux...");
    await FluxCLIWrapper.InstallAsync();
    await FluxCLIWrapper.CreateSourceOCIAsync(sourceUrl);
    await FluxCLIWrapper.CreateKustomizationAsync(fluxKustomizationPathOption);
    Console.WriteLine("🔄✅ Flux installed successfully...");
  }

  public async Task UninstallAsync()
  {
    Console.WriteLine("🔄 Uninstalling Flux...");
    await FluxCLIWrapper.UninstallAsync();
    Console.WriteLine("🔄✅ Flux uninstalled successfully...");
  }
}
