using Devantler.KubernetesGenerator.Core.Converters;
using Devantler.KubernetesGenerator.Core.Inspectors;
using KSail.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.System.Text.Json;

namespace KSail;

static class KSailClusterConfigLoader
{

  internal static async Task<KSailCluster> LoadAsync(string? name)
  {
    var ksailClusterConfig = new KSailCluster();
    string currentDirectory = Directory.GetCurrentDirectory();
    string[] possibleFiles = [
      "ksail-cluster.yaml",
      "ksail-config.yaml",
      "ksail.yaml",
      ".ksail.yaml"
    ];
    string? ksailYaml = FindConfigFile(currentDirectory, possibleFiles);

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
