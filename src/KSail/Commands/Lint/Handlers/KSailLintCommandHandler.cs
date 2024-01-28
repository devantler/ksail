using System.Formats.Tar;
using KSail.CLIWrappers;
using KSail.Exceptions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace KSail.Commands.Lint.Handlers;

class KSailLintCommandHandler()
{
  static readonly HttpClient httpClient = new();
  internal static async Task HandleAsync(string clusterName, string manifestsPath)
  {
    Console.WriteLine("🧹 Linting manifest files...");

    Console.WriteLine("► Downloading Flux OpenAPI schemas...");
    const string url = "https://github.com/fluxcd/flux2/releases/latest/download/crd-schemas.tar.gz";
    var directoryInfo = Directory.CreateDirectory("/tmp/flux-crd-schemas/master-standalone-strict");
    await using (var file = await httpClient.GetStreamAsync(url).ConfigureAwait(false))
    await using (var memoryStream = new MemoryStream())
    {
      await TarFile.ExtractToDirectoryAsync(memoryStream, directoryInfo.FullName, true);
    }

    ValidateYaml(manifestsPath);
    if (string.IsNullOrEmpty(clusterName))
    {
      foreach (string clusterPath in Directory.GetDirectories($"{manifestsPath}/clusters"))
      {
        string name = clusterPath.Replace($"{manifestsPath}/clusters/", "", StringComparison.Ordinal);
        await ValidateKustomizationsAsync(name, manifestsPath);
      }
    }
    else
    {
      await ValidateKustomizationsAsync(clusterName, manifestsPath);
    }
    Console.WriteLine("");
  }

  static void ValidateYaml(string manifestsPath)
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
        catch (YamlException)
        {
          throw new YamlException($"✕ Validation failed for {manifest}...");
        }
      }
    }
    catch (YamlException e)
    {
      throw new YamlException($"🚨 An error occurred while validating YAML files: {e.Message}...");
    }
  }

  //TODO: Refactor the ValidateKustomizationsAsync method
  // Move the CLI commands to an appropriate CLIWrapper class.
  // Extract methods
  // Consider a helper class
  static async Task ValidateKustomizationsAsync(string clusterName, string manifestsPath)
  {
    string[] kubeconformFlags = ["-skip=Secret"];
    string[] kubeconformConfig = ["-strict", "-ignore-missing-schemas", "-schema-location", "default", "-schema-location", "/tmp/flux-crd-schemas", "-verbose"];

    string clusterPath = $"{manifestsPath}/clusters/{clusterName}";
    if (!Directory.Exists(clusterPath))
    {
      throw new DirectoryNotFoundException($"🚨 Cluster '{clusterName}' not found in path '{clusterPath}'...");
    }
    Console.WriteLine($"► Validating cluster '{clusterName}' with Kubeconform...");
    foreach (string manifest in Directory.GetFiles(clusterPath, "*.yaml", SearchOption.AllDirectories))
    {
      await KubeconformCLIWrapper.Run(kubeconformFlags, kubeconformConfig, manifest);
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
      try
      {
        _ = await CLIRunner.RunAsync(cmd);
      }
      catch (InvalidOperationException)
      {
        Console.WriteLine($"✕ Validation failed for '{manifest}'...");
        throw new KSailException($"✕ Validation failed for '{manifest}'...");
      }
    }
  }
}
