using Devantler.KubernetesGenerator.Core.Converters;
using Devantler.KubernetesGenerator.KSail.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.System.Text.Json;

namespace KSail.Deserializer;

class KSailClusterDeserializer
{
  readonly IDeserializer _deserializer = new DeserializerBuilder()
    .WithTypeInspector(inner => new SystemTextJsonTypeInspector(inner))
    .WithTypeConverter(new IntstrIntOrStringTypeConverter())
    .WithTypeConverter(new ResourceQuantityTypeConverter())
    .WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

  internal async Task<KSailCluster> LocateAndDeserializeAsync()
  {
    var ksailClusterConfig = new KSailCluster();
    string currentDirectory = Directory.GetCurrentDirectory();
    string ksailYaml = Path.Combine(currentDirectory, "ksail-config.yaml");
    if (!File.Exists(ksailYaml))
    {
      var parentDirectory = Directory.GetParent(currentDirectory);
      while (parentDirectory != null)
      {
        ksailYaml = Path.Combine(parentDirectory.FullName, "ksail-config.yaml");
        if (File.Exists(ksailYaml))
          break;
        parentDirectory = parentDirectory.Parent;
      }
    }
    if (File.Exists(ksailYaml))
      ksailClusterConfig = _deserializer.Deserialize<KSailCluster>(await File.ReadAllTextAsync(ksailYaml).ConfigureAwait(false));
    return ksailClusterConfig;
  }
}
