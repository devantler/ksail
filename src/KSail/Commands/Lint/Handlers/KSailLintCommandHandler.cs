using System.Formats.Tar;
using KSail.CLIWrappers;
using YamlDotNet.Serialization;

namespace KSail.Commands.Lint.Handlers;

static class KSailLintCommandHandler
{
  static readonly HttpClient _httpClient = new();
  internal static async Task Handle(string manifestsPath)
  {
    Console.WriteLine($"🔎 Linting files in '{manifestsPath}'...");
    Console.WriteLine("🔎 Downloading Flux OpenAPI schemas...");
    const string url = "https://github.com/fluxcd/flux2/releases/latest/download/crd-schemas.tar.gz";
    var directoryInfo = Directory.CreateDirectory("/tmp/flux-crd-schemas/master-standalone-strict");
    await using (var file = await _httpClient.GetStreamAsync(url).ConfigureAwait(false))
    await using (var memoryStream = new MemoryStream())
    {
      await TarFile.ExtractToDirectoryAsync(memoryStream, directoryInfo.FullName, true);
    }
    Console.WriteLine("✅ Flux OpenAPI schemas downloaded successfully...");

    ValidateYaml(manifestsPath);
    ValidateKustomizations(manifestsPath);
  }

  static void ValidateYaml(string manifestsPath)
  {
    foreach (string manifest in Directory.GetFiles(manifestsPath, "*.yaml", SearchOption.AllDirectories))
    {
      Console.Write($"🔎 Validating {manifest}...");
      var deserializer = new Deserializer();
      try
      {
        _ = deserializer.Deserialize(new StringReader(File.ReadAllText(manifest)));
      }
      catch (Exception e)
      {
        Console.WriteLine($"🚨 {e.Message}");
        Environment.Exit(1);
      }
      Console.WriteLine(" ✅");
    }
  }

  static void ValidateKustomizations(string manifestsPath)
  {
    string[] kubeconformFlags = ["-skip=Secret"];
    string[] kubeconformConfig = ["-strict", "-ignore-missing-schemas", "-schema-location", "default", "-schema-location", "/tmp/flux-crd-schemas", "-verbose"];

    string clustersPath = $"{manifestsPath}/clusters";
    Console.WriteLine("🔎 Validating cluster manifests...");
    foreach (string cluster in Directory.GetDirectories(clustersPath))
    {
      foreach (string manifest in Directory.GetFiles(cluster, "*.yaml", SearchOption.AllDirectories))
      {
        KubeconformCLIWrapper.Run(kubeconformFlags, kubeconformConfig, manifest);
      }
    }

    string[] kustomizeFlags = ["--load-restrictor=LoadRestrictionsNone"];
    const string kustomization = "kustomization.yaml";
    Console.WriteLine("🔎 Validating kustomizations...");
    foreach (string manifest in Directory.GetFiles(manifestsPath, kustomization, SearchOption.AllDirectories))
    {
      string kustomizationPath = manifest.Replace(kustomization, "");
      Console.WriteLine($"🔎 Validating kustomization {kustomizationPath}...");
      var kustomizeBuildCmd = KustomizeCLIWrapper.Kustomize.WithArguments($"build {kustomizationPath} {kustomizeFlags}");
      var kubeconformCmd = KubeconformCLIWrapper.Kubeconform.WithArguments(kubeconformFlags.Concat(kubeconformConfig).ToArray());
      var cmd = kustomizeBuildCmd | kubeconformCmd;
      _ = CLIRunner.RunAsync(cmd);
    }
  }
}
