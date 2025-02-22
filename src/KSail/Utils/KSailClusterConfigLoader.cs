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
  static readonly IDeserializer _deserializer = new DeserializerBuilder()
      .WithTypeInspector(inner => new KubernetesTypeInspector(new SystemTextJsonTypeInspector(inner)))
      .WithTypeConverter(new IntstrIntOrStringTypeConverter())
      .WithTypeConverter(new ResourceQuantityTypeConverter())
      .WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

  internal static async Task<KSailCluster> LoadAsync(string? configFilePath = "ksail-config.yaml", string? directory = default, string? name = default, KSailKubernetesDistribution distribution = default)
  {
    // Create default KSailClusterConfig
    var ksailClusterConfig = string.IsNullOrEmpty(name) ?
      new KSailCluster(distribution: distribution) :
      new KSailCluster(name, distribution: distribution);

    // Locate KSail YAML file
    string startDirectory = directory ?? Directory.GetCurrentDirectory();
    string? ksailYaml = string.IsNullOrEmpty(configFilePath) ?
      FindConfigFile(startDirectory, "ksail-config.yaml") :
      FindConfigFile(startDirectory, configFilePath);


    // If no KSail YAML file is found, return the default KSailClusterConfig
    if (ksailYaml == null)
    {
      return ksailClusterConfig;
    }

    // Deserialize KSail YAML file
    ksailClusterConfig = _deserializer.Deserialize<KSailCluster>(await File.ReadAllTextAsync(ksailYaml).ConfigureAwait(false));

    return ksailClusterConfig;
  }

  static string? FindConfigFile(string startDirectory, string configFilePath)
  {
    if (Path.IsPathRooted(configFilePath))
    {
      return File.Exists(configFilePath) ? configFilePath : null;
    }
    string? currentDirectory = startDirectory;
    while (currentDirectory != null)
    {
      string filePath = Path.Combine(currentDirectory, configFilePath);
      if (File.Exists(filePath))
        return filePath;
      var parentDirectory = Directory.GetParent(currentDirectory);
      currentDirectory = parentDirectory?.FullName;
    }
    return null;
  }
}
