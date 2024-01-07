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
    var directoryInfo = Directory.CreateDirectory("/tmp/flux-crd-schemas/master-standalone-strict");
    var stream = await _httpClient.GetStreamAsync("https://github.com/fluxcd/flux2/releases/latest/download/crd-schemas.tar.gz");
    TarFile.ExtractToDirectory(stream, directoryInfo.FullName, true);
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

  // Implement this:
  // echo "🔍 INFO - Validating clusters"
  // find ./k8s/clusters -maxdepth 2 -type f -name '*.yaml' -print0 | while IFS= read -r -d $'\0' file; do
  //   kubeconform "${kubeconform_flags[@]}" "${kubeconform_config[@]}" "${file}"
  //   if [[ ${PIPESTATUS[0]} != 0 ]]; then
  //     exit 1
  //   fi
  // done
  // echo "🔍 INFO - Validating kustomize overlays"
  // find . -type f -name $kustomize_config -print0 | while IFS= read -r -d $'\0' file; do
  //   echo "🔍 INFO - Validating kustomization ${file/%$kustomize_config/}"
  //   kustomize build "${file/%$kustomize_config/}" "${kustomize_flags[@]}" |
  //     kubeconform "${kubeconform_flags[@]}" "${kubeconform_config[@]}"
  //   if [[ ${PIPESTATUS[0]} != 0 ]]; then
  //     exit 1
  //   fi
  // done
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
