using KSail.CLIWrappers;

namespace KSail.Provisioners.GitOps;

sealed class FluxProvisioner : IGitOpsProvisioner
{
  public async Task<int> InstallAsync(string context, string sourceUrl, string path, CancellationToken token)
  {
    Console.WriteLine("🔼 Checking Flux prerequisites are satisfied...");
    if (await FluxCLIWrapper.CheckPrerequisitesAsync(context, token) != 0)
    {
      Console.WriteLine("✕ Flux prerequisites are not satisfied");
      return 1;
    }
    Console.WriteLine();

    Console.WriteLine("🔼 Installing Flux...");
    if (await FluxCLIWrapper.InstallAsync(context, token) != 0)
    {
      Console.WriteLine("✕ Failed to install Flux");
      return 1;
    }
    Console.WriteLine();

    Console.WriteLine("🔼 Creating Flux OCI source...");
    if (await FluxCLIWrapper.CreateSourceOCIAsync(context, sourceUrl, token) != 0)
    {
      Console.WriteLine("✕ Failed to create Flux OCI source");
      return 1;
    }
    Console.WriteLine();

    Console.WriteLine("🔼 Creating Flux kustomization...");
    if (await FluxCLIWrapper.CreateKustomizationAsync(context, path, token) != 0)
    {
      Console.WriteLine("✕ Failed to create Flux kustomization");
      return 1;
    }
    Console.WriteLine();
    return 0;
  }
  public async Task<int> UninstallAsync(string context, CancellationToken token)
  {
    Console.WriteLine("🚮 Uninstalling Flux...");
    if (await FluxCLIWrapper.UninstallAsync(context, token) != 0)
    {
      Console.WriteLine("✕ Failed to uninstall Flux");
      return 1;
    }
    Console.WriteLine();
    return 0;
  }
  public async Task<int> ReconcileAsync(string context, CancellationToken token)
  {
    Console.WriteLine("🔄 Reconciling Flux...");
    if (await FluxCLIWrapper.ReconcileAsync(context, token) != 0)
    {
      Console.WriteLine("✕ Failed to reconcile Flux");
      return 1;
    }
    Console.WriteLine();
    return 0;
  }
  public async Task<int> PushManifestsAsync(string ociUrl, string manifestsPath, CancellationToken token)
  {
    Console.WriteLine("📥 Pushing manifests...");
    if (await FluxCLIWrapper.PushManifestsAsync(ociUrl, manifestsPath, token) != 0)
    {
      return 1;
    }
    Console.WriteLine();
    return 0;
  }
}
