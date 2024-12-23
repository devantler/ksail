using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;

namespace KSail.Models.Tests;

/// <summary>
/// Tests that a JSONSchema is generate from the <see cref="KSailCluster"/> object
/// </summary>
public class KSailClusterJSONSchemaGeneration
{
  /// <summary>
  /// Tests that a JSONSchema is generated from the <see cref="KSailCluster"/> object.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task GenerateJSONSchemaFromKSailCluster_ShouldReturnJSONSchema()
  {
    // Arrange & Act
    var options = JsonSerializerOptions.Default;
    var schema = new JsonObject
    {
      ["$schema"] = "https://json-schema.org/draft-07/schema#",
      ["$id"] = "https://raw.githubusercontent.com/devantler/ksail/main/schemas/ksail-cluster-schema.json",
      ["title"] = "KSail Cluster",
      ["description"] = "A configuration object for a KSail cluster"
    };
    var ksailSchema = options.GetJsonSchemaAsNode(typeof(KSailCluster));
    foreach (var property in ksailSchema.AsObject())
    {
      if (!schema.ContainsKey(property.Key))
      {
        schema[property.Key] = property.Value?.DeepClone();
      }
    }

    // Assert
    _ = await Verify(schema.ToString());
    await File.WriteAllTextAsync("../../../../../../schemas/ksail-cluster-schema.json", schema.ToString());
  }
}
