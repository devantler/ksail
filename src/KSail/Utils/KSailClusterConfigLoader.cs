using Devantler.KubernetesGenerator.Core.Converters;
using Devantler.KubernetesGenerator.Core.Inspectors;
using KSail.Models;
using KSail.Models.Project;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.System.Text.Json;

namespace KSail.Utils;

static class KSailClusterConfigLoader
{

  internal static async Task<KSailCluster> LoadAsync(string? directory = default, string? name = default, KSailKubernetesDistribution distribution = KSailKubernetesDistribution.Kind)
  {
    var ksailClusterConfig = name != null ? new KSailCluster(name, distribution: distribution) : new KSailCluster(distribution: distribution);
    directory ??= Directory.GetCurrentDirectory();
    string[] possibleFiles = [
      "ksail-cluster.yaml",
      "ksail-config.yaml",
      "ksail.yaml",
      ".ksail.yaml"
    ];
    string? ksailYaml = FindConfigFile(directory, possibleFiles);

    if (ksailYaml != null)
    {
      var deserializer = new DeserializerBuilder()
        .WithTypeInspector(inner => new KubernetesTypeInspector(new SystemTextJsonTypeInspector(inner)))
        .WithTypeConverter(new IntstrIntOrStringTypeConverter())
        .WithTypeConverter(new ResourceQuantityTypeConverter())
        .WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

      ksailClusterConfig = deserializer.Deserialize<KSailCluster>(await File.ReadAllTextAsync(ksailYaml).ConfigureAwait(false));
    }
    return ksailClusterConfig;
  }

  static string? FindConfigFile(string startDirectory, string[] possibleFiles)
  {
    string? currentDirectory = startDirectory;
    while (currentDirectory != null)
    {
      foreach (string file in possibleFiles)
      {
        string filePath = Path.Combine(currentDirectory, file);
        if (File.Exists(filePath))
          return filePath;
      }
      var parentDirectory = Directory.GetParent(currentDirectory);
      currentDirectory = parentDirectory?.FullName;
    }
    return null;
  }
}
