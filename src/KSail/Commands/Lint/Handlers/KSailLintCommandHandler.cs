using System.Formats.Tar;
using KSail.CLIWrappers;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace KSail.Commands.Lint.Handlers;

internal class KSailLintCommandHandler()
{
  private static readonly HttpClient httpClient = new();
  internal static async Task HandleAsync(string name, string manifestsPath)
  {
    Console.WriteLine("🧹 Linting manifest files...");

    if (string.IsNullOrEmpty(name))
    {
      Console.WriteLine("✕ Name of the cluster is required...");
      Environment.Exit(1);
    }

    Console.WriteLine("► Downloading Flux OpenAPI schemas...");
    const string url = "https://github.com/fluxcd/flux2/releases/latest/download/crd-schemas.tar.gz";
    var directoryInfo = Directory.CreateDirectory("/tmp/flux-crd-schemas/master-standalone-strict");
    await using (var file = await httpClient.GetStreamAsync(url).ConfigureAwait(false))
    await using (var memoryStream = new MemoryStream())
    {
      await TarFile.ExtractToDirectoryAsync(memoryStream, directoryInfo.FullName, true);
    }

    ValidateYaml(manifestsPath);
    await ValidateKustomizationsAsync(name, manifestsPath);
    Console.WriteLine("");
  }

  private static void ValidateYaml(string manifestsPath)
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
        catch (YamlException e)
        {
          Console.WriteLine($"✕ Validation failed for {manifest}. {e.Message}...");
          Environment.Exit(1);
        }
      }
    }
    catch (YamlException e)
    {
      Console.WriteLine($"🚨 An error occurred while validating YAML files: {e.Message}...");
      Environment.Exit(1);
    }
  }

  private static async Task ValidateKustomizationsAsync(string name, string manifestsPath)
  {
    string[] kubeconformFlags = ["-skip=Secret"];
    string[] kubeconformConfig = ["-strict", "-ignore-missing-schemas", "-schema-location", "default", "-schema-location", "/tmp/flux-crd-schemas", "-verbose"];

    string clusterPath = $"{manifestsPath}/clusters/{name}";
    if (!Directory.Exists(clusterPath))
    {
      Console.WriteLine($"🚨 Cluster '{name}' not found in path '{clusterPath}'...");
      Environment.Exit(1);
    }
    Console.WriteLine($"► Validating cluster '{name}' with Kubeconform...");
    foreach (string manifest in Directory.GetFiles(clusterPath, "*.yaml", SearchOption.AllDirectories))
    {
      KubeconformCLIWrapper.Run(kubeconformFlags, kubeconformConfig, manifest);
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
      _ = await CLIRunner.RunAsync(cmd);
    }
  }
}
