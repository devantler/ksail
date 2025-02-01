using Argon;
using KSail.Models.Project;

namespace KSail.Models.Tests;

/// <summary>
/// Tests for <see cref="KSailCluster"/> object initialization.
/// </summary>
public class KSailClusterInitialization
{
  /// <summary>
  /// Tests that <see cref="KSailCluster"/> object is initialized with default values.
  /// </summary>
  [Fact]
  public async Task InitializeKSailCluster_WithNoInput_ShouldReturnValidConfig()
  {
    // Arrange
    var cluster = new KSailCluster();

    // Act & Assert
    cluster.Spec.FluxDeploymentToolOptions.Source.Url = new Uri("oci://testhost:5555/ksail-registry");
    var settings = new VerifySettings();
    settings.AddExtraSettings(s => s.DefaultValueHandling = DefaultValueHandling.Include);
    settings.DontIgnoreEmptyCollections();
    _ = await Verify(cluster, settings);
  }

  /// <summary>
  /// Tests that <see cref="KSailCluster"/> object is initialized with a name.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task InitializeKSailCluster_WithName_ShouldReturnValidConfig()
  {
    // Arrange
    var cluster = new KSailCluster("my-cluster");

    // Act & Assert
    var settings = new VerifySettings();
    settings.AddExtraSettings(s =>
    {
      s.DefaultValueHandling = DefaultValueHandling.Include;
    });
    cluster.Spec.FluxDeploymentToolOptions.Source.Url = new Uri("oci://testhost:5555/ksail-registry");
    settings.DontIgnoreEmptyCollections();
    _ = await Verify(cluster, settings);
  }

  /// <summary>
  /// Tests that <see cref="KSailCluster"/> object is initialized with a distribution.
  /// </summary>
  [Fact]
  public async Task InitializeKSailCluster_WithDistribution_ShouldReturnValidConfig()
  {
    // Arrange
    var cluster = new KSailCluster(KSailKubernetesDistribution.K3s);

    // Act & Assert
    var settings = new VerifySettings();
    settings.AddExtraSettings(s =>
    {
      s.DefaultValueHandling = DefaultValueHandling.Include;
    });
    cluster.Spec.FluxDeploymentToolOptions.Source.Url = new Uri("oci://testhost:5555/ksail-registry");
    settings.DontIgnoreEmptyCollections();
    _ = await Verify(cluster, settings);
  }

  /// <summary>
  /// Tests that <see cref="KSailCluster"/> object is initialized with a name and distribution.
  /// </summary>
  [Fact]
  public async Task InitializeKSailCluster_WithNameAndDistribution_ShouldReturnValidConfig()
  {
    // Arrange
    var cluster = new KSailCluster("my-cluster", KSailKubernetesDistribution.K3s);

    // Act & Assert
    var settings = new VerifySettings();
    settings.AddExtraSettings(s =>
    {
      s.DefaultValueHandling = DefaultValueHandling.Include;
    });
    cluster.Spec.FluxDeploymentToolOptions.Source.Url = new Uri("oci://testhost:5555/ksail-registry");
    settings.DontIgnoreEmptyCollections();
    _ = await Verify(cluster, settings);
  }
}
