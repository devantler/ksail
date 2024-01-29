using System.Formats.Tar;
using KSail.CLIWrappers;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace KSail.Commands.Lint.Handlers;

class KSailLintCommandHandler()
{
  static readonly HttpClient httpClient = new();
  internal static async Task<int> HandleAsync(string clusterName, string manifestsPath, CancellationToken token)
  {
    Console.WriteLine("🧹 Linting manifest files...");

    Console.WriteLine("► Downloading Flux OpenAPI schemas...");
    const string url = "https://github.com/fluxcd/flux2/releases/latest/download/crd-schemas.tar.gz";
    var directoryInfo = Directory.CreateDirectory("/tmp/flux-crd-schemas/master-standalone-strict");
    await using (var file = await httpClient.GetStreamAsync(url, token).ConfigureAwait(false))
    await using (var memoryStream = new MemoryStream())
    {
      await TarFile.ExtractToDirectoryAsync(memoryStream, directoryInfo.FullName, true, token);
    }

    if (await ValidateYamlAsync(manifestsPath) != 0 || await ValidateKustomizationsAsync(clusterName, manifestsPath, token) != 0)
    {
      return 1;
    }
    Console.WriteLine("");
    return 0;
  }

  static Task<int> ValidateYamlAsync(string manifestsPath)
  {
    Console.WriteLine("► Validating YAML files with YAMLDotNet...");
    try
    {
      foreach (string manifest in Directory.GetFiles(manifestsPath, "*.yaml", SearchOption.AllDirectories))
      {
        var input = new StringReader(manifest);
        var parser = new Parser(input);
        var deserializer = new Deserializer();
        try
        {
          _ = parser.Consume<StreamStart>();

          while (parser.Accept<DocumentStart>(out var @event))
          {
            object? doc = deserializer.Deserialize(parser);
          }
        }
        catch (Exception)
        {
          Console.WriteLine($"✕ Validation failed for {manifest}...");
          return Task.FromResult(1);
        }
      }
    }
    catch (Exception e)
    {
      Console.WriteLine($"✕ An error occurred while validating YAML files: {e.Message}...");
      return Task.FromResult(1);
    }
    return Task.FromResult(0);
  }

  //TODO: Refactor the ValidateKustomizationsAsync method
  // Move the CLI commands to an appropriate CLIWrapper class.
  // Extract methods
  // Consider a helper class
  static async Task<int> ValidateKustomizationsAsync(string clusterName, string manifestsPath, CancellationToken token)
  {
    string[] kubeconformFlags = ["-skip=Secret"];
    string[] kubeconformConfig = ["-strict", "-ignore-missing-schemas", "-schema-location", "default", "-schema-location", "/tmp/flux-crd-schemas", "-verbose"];

    string clusterPath = $"{manifestsPath}/clusters/{clusterName}";
    if (!Directory.Exists(clusterPath))
    {
      Console.WriteLine($"✕ Cluster '{clusterName}' not found in path '{clusterPath}'...");
      return 1;
    }
    Console.WriteLine($"► Validating cluster '{clusterName}' with Kubeconform...");
    foreach (string manifest in Directory.GetFiles(clusterPath, "*.yaml", SearchOption.AllDirectories))
    {
      if (await KubeconformCLIWrapper.RunAsync(kubeconformFlags, kubeconformConfig, manifest, token) != 0)
      {
        Console.WriteLine($"✕ Validation failed for '{manifest}'...");
        return 1;
      }
    }

    string[] kustomizeFlags = ["--load-restrictor=LoadRestrictionsNone"];
    const string kustomization = "kustomization.yaml";
    Console.WriteLine("► Validating kustomizations with Kustomize and Kubeconform...");
    foreach (string manifest in Directory.GetFiles(manifestsPath, kustomization, SearchOption.AllDirectories))
    {
      string kustomizationPath = manifest.Replace(kustomization, "", StringComparison.Ordinal);
      var kustomizeBuildCmd = KustomizeCLIWrapper.Kustomize.WithArguments(["build", kustomizationPath, .. kustomizeFlags]);
      var kubeconformCmd = KubeconformCLIWrapper.Kubeconform.WithArguments([.. kubeconformFlags, .. kubeconformConfig]);
      var cmd = kustomizeBuildCmd | kubeconformCmd;
      var (ExitCode, _) = await CLIRunner.RunAsync(cmd, token);
      if (ExitCode != 0)
      {
        Console.WriteLine($"✕ Validation failed for '{kustomizationPath}'...");
        return 1;
      }
    }
    return 0;
  }
}
