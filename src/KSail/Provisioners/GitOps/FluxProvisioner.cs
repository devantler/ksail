using KSail.CLIWrappers;

namespace KSail.Provisioners.GitOps;

sealed class FluxProvisioner : IGitOpsProvisioner
{
  public async Task InstallAsync(string context, string sourceUrl, string path)
  {
    Console.WriteLine("🔄 Checking Flux prerequisites are satisfied...");
    await FluxCLIWrapper.CheckPrerequisitesAsync(context);
    Console.WriteLine();
    Console.WriteLine("🔄 Installing Flux...");
    await FluxCLIWrapper.InstallAsync(context);
    Console.WriteLine();
    Console.WriteLine("🔄 Creating Flux OCI source...");
    await FluxCLIWrapper.CreateSourceOCIAsync(context, sourceUrl);
    Console.WriteLine();
    Console.WriteLine("🔄 Creating Flux kustomization...");
    await FluxCLIWrapper.CreateKustomizationAsync(context, path);
    Console.WriteLine();
  }
  public async Task UninstallAsync(string context)
  {
    Console.WriteLine("🔄 Uninstalling Flux...");
    await FluxCLIWrapper.UninstallAsync(context);
    Console.WriteLine();
  }
  public async Task ReconcileAsync(string context)
  {
    Console.WriteLine("🔄 Reconciling Flux...");
    await FluxCLIWrapper.ReconcileAsync(context);
    Console.WriteLine();
  }
  public async Task PushManifestsAsync(string context, string ociUrl, string manifestsPath)
  {
    Console.WriteLine("🔄 Pushing manifests...");
    await FluxCLIWrapper.PushManifestsAsync(context, ociUrl, manifestsPath);
    Console.WriteLine();
  }
}
