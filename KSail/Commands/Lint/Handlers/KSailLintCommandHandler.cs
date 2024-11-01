using Devantler.CLIRunner;
using Devantler.KubeconformCLI;
using Devantler.KustomizeCLI;
using KSail.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace KSail.Commands.Lint.Handlers;

class KSailLintCommandHandler()
{
  internal static async Task<int> HandleAsync(KSailCluster config, CancellationToken cancellationToken) =>
    ValidateYaml(config) &&
    await ValidateKustomizationsAsync(config, cancellationToken).ConfigureAwait(false) ?
    0 : 1;

  static bool ValidateYaml(KSailCluster config)
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
        catch (YamlException e)
        {
          Console.WriteLine($"✕ {e.Message}");
          return false;
        }
      }
    }
    catch (ArgumentException e)
    {
      Console.WriteLine($"✕ {e.Message}");
      return false;
    }
    catch (DirectoryNotFoundException e)
    {
      Console.WriteLine($"✕ {e.Message}");
      return false;
    }
    Console.WriteLine("✔ YAML files are valid");
    Console.WriteLine("");
    return true;
  }

  //TODO: Refactor the ValidateKustomizationsAsync method
  // Move the CLI commands to an appropriate CLIWrapper class.
  // Extract methods
  // Consider a helper class
  static async Task<bool> ValidateKustomizationsAsync(KSailCluster config, CancellationToken cancellationToken)
  {
    string[] kubeconformFlags = ["-ignore-filename-pattern=.*\\.sops\\.yaml"];
    string[] kubeconformConfig = ["-strict", "-ignore-missing-schemas", "-schema-location", "default", "-schema-location", "https://raw.githubusercontent.com/datreeio/CRDs-catalog/main/{{.Group}}/{{.ResourceKind}}_{{.ResourceAPIVersion}}.json", "-verbose"];
    bool isValid = await RunKubeconform(config, kubeconformFlags, kubeconformConfig, cancellationToken).ConfigureAwait(false);

    if (!isValid)
    {
      return false;
    }

    isValid = await RunKubeconformAndKustomize(config, kubeconformFlags, kubeconformConfig, cancellationToken).ConfigureAwait(false);
    return isValid;
  }

  static async Task<bool> RunKubeconformAndKustomize(KSailCluster config, string[] kubeconformFlags, string[] kubeconformConfig, CancellationToken cancellationToken)
  {
    string[] kustomizeFlags = ["--load-restrictor=LoadRestrictionsNone"];
    const string Kustomization = "kustomization.yaml";
    Console.WriteLine("► Validating kustomizations with Kustomize and Kubeconform");
    foreach (string manifest in Directory.GetFiles(config.Spec.ManifestsDirectory, Kustomization, SearchOption.AllDirectories))
    {
      string kustomizationPath = manifest.Replace(Kustomization, "", StringComparison.Ordinal);
      var kustomizeBuildCmd = Kustomize.Command.WithArguments(["build", kustomizationPath, .. kustomizeFlags]);
      var kubeconformCmd = Kubeconform.Command.WithArguments([.. kubeconformFlags, .. kubeconformConfig]);
      var cmd = kustomizeBuildCmd | kubeconformCmd;
      var (exitCode, _) = await CLI.RunAsync(cmd, cancellationToken: cancellationToken).ConfigureAwait(false);
      if (exitCode != 0)
      {
        Console.WriteLine($"✕ Validation failed for '{kustomizationPath}'");
        return false;
      }
    }
    return true;
  }

  static async Task<bool> RunKubeconform(KSailCluster config, string[] kubeconformFlags, string[] kubeconformConfig, CancellationToken cancellationToken)
  {
    string clusterPath = $"{config.Spec.ManifestsDirectory}/clusters/{config.Metadata.Name}";
    if (!Directory.Exists(clusterPath))
    {
      Console.WriteLine($"✕ Cluster '{config.Metadata.Name}' not found in path '{clusterPath}'");
      return false;
    }
    Console.WriteLine($"► Validating cluster '{config.Metadata.Name}' with Kubeconform");
    foreach (string file in Directory.GetFiles(clusterPath, "*.yaml", SearchOption.AllDirectories))
    {
      try
      {
        await Kubeconform.RunAsync(file, kubeconformFlags, kubeconformConfig, cancellationToken).ConfigureAwait(false);
      }
      catch (KubeconformException e)
      {
        Console.WriteLine($"✕ {e.Message}");
        return false;
      }
    }
    return true;
  }
}
