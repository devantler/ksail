using KSail.CLIWrappers;
using KSail.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace KSail.Commands.Lint.Handlers;

class KSailLintCommandHandler()
{
  internal static async Task HandleAsync(KSailCluster config, CancellationToken cancellationToken)
  {
    Console.WriteLine("🧹 Linting manifest files");
    await ValidateYamlAsync(config).ConfigureAwait(false);
    await ValidateKustomizationsAsync(config, cancellationToken).ConfigureAwait(false);
    Console.WriteLine("");
  }

  static Task ValidateYamlAsync(KSailCluster config)
  {
    Console.WriteLine("► Validating YAML files with YAMLDotNet");
    try
    {
      foreach (string manifest in Directory.GetFiles(config.Spec.ManifestsDirectory, "*.yaml", SearchOption.AllDirectories))
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
          throw new KSailException("YAML validation failed for {manifest}");
        }
      }
    }
    catch (ArgumentException e)
    {
      throw new KSailException($"An error occurred while validating YAML files: {e.Message}");
    }
  }

  //TODO: Refactor the ValidateKustomizationsAsync method
  // Move the CLI commands to an appropriate CLIWrapper class.
  // Extract methods
  // Consider a helper class
  static async Task ValidateKustomizationsAsync(KSailCluster config, CancellationToken cancellationToken)
  {
    string[] kubeconformFlags = ["-skip=Secret"];
    string[] kubeconformConfig = ["-strict", "-ignore-missing-schemas", "-schema-location", "default", "-schema-location", "https://raw.githubusercontent.com/datreeio/CRDs-catalog/main/{{.Group}}/{{.ResourceKind}}_{{.ResourceAPIVersion}}.json", "-verbose"];

    string clusterPath = $"{config.Spec.ManifestsDirectory}/clusters/{config.Metadata.Name}";
    if (!Directory.Exists(clusterPath))
    {
      throw new KSailException($"Cluster '{config.Metadata.Name}' not found in path '{clusterPath}'");
    }
    Console.WriteLine($"► Validating cluster '{config.Metadata.Name}' with Kubeconform");
    foreach (string manifest in Directory.GetFiles(clusterPath, "*.yaml", SearchOption.AllDirectories))
    {
      if (await KubeconformCLIWrapper.RunAsync(kubeconformFlags, kubeconformConfig, manifest, cancellationToken).ConfigureAwait(false) != 0)
      {
        throw new KSailException($"Validation failed for '{manifest}'");
      }
    }

    string[] kustomizeFlags = ["--load-restrictor=LoadRestrictionsNone"];
    const string kustomization = "kustomization.yaml";
    Console.WriteLine("► Validating kustomizations with Kustomize and Kubeconform");
    foreach (string manifest in Directory.GetFiles(manifestsPath, kustomization, SearchOption.AllDirectories))
    {
      string kustomizationPath = manifest.Replace(kustomization, "", StringComparison.Ordinal);
      var kustomizeBuildCmd = KustomizeCLIWrapper.Kustomize.WithArguments(["build", kustomizationPath, .. kustomizeFlags]);
      var kubeconformCmd = KubeconformCLIWrapper.Kubeconform.WithArguments([.. kubeconformFlags, .. kubeconformConfig]);
      var cmd = kustomizeBuildCmd | kubeconformCmd;
      var (exitCode, _) = await CLIRunner.RunAsync(cmd, cancellationToken).ConfigureAwait(false);
      if (exitCode != 0)
      {
        throw new KSailException($"Validation failed for '{kustomizationPath}'");
      }
    }
    return 0;
  }
}
