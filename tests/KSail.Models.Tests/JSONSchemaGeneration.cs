using System.Text.Json;
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
    // Arrange
    var options = JsonSerializerOptions.Default;
    var schema = options.GetJsonSchemaAsNode(typeof(KSailCluster));

    // Act
    // Assert
    _ = await Verify(schema.ToString());
    await File.WriteAllTextAsync("../../../../../../schemas/ksail-cluster-schema.json", schema.ToString());
  }
}
