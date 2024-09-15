using Devantler.KubernetesGenerator.KSail.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KSail.Deserializer;

class KSailClusterConfigDeserializer
{
  readonly IDeserializer _deserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .Build();

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
