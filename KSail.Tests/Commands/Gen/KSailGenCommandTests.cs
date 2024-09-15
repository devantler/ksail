using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Gen;

namespace KSail.Tests.Commands.Gen;

/// <summary>
/// Tests for the <see cref="KSailGenCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailGenCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the 'ksail gen' command succeeds and returns the introduction and help text.
  /// </summary>
  [Fact]
  public async Task KSailGen_SucceedsAndPrintsIntroductionAndHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailGenCommand(console);

    //Act
    int exitCode = await ksailCommand.InvokeAsync("", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the 'ksail gen --help' command succeeds and returns the help text.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task KSailGenHelp_SucceedsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailGenCommand(console);

    //Act
    int exitCode = await ksailCommand.InvokeAsync("--help", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the 'ksail gen config' command succeeds and returns the help text.
  /// </summary>
  [Fact]
  public async Task KSailGenConfig_SucceedsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailGenCommand(console);

    //Act
    int exitCode = await ksailCommand.InvokeAsync("config", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the 'ksail gen config k3d' command generates a 'k3d.io/v1alpha5/Simple' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenConfigK3d_SucceedsAndGeneratesAK3dConfigurationFile()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "k3d-config.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"config k3d --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen config ksail' command generates a 'ksail.io/v1alpha1/Cluster' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenConfigKSail_SucceedsAndGeneratesAKSailConfigurationFile()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "ksail-config.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"config ksail --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen config sops' command generates a SOPS configuration file.
  /// </summary>
  [Fact]
  public async Task KSailGenConfigSops_SucceedsAndGeneratesASOPSConfigurationFile()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), ".sops.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"config sops --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen flux kustomization' command generates a 'kustomize.toolkit.fluxcd.io/v1/Kustomization' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenFluxKustomization_SucceedsAndGeneratesAFluxKustomizationFile()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "flux-kustomization.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"flux kustomization --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen kustomize component' command generates a 'kustomize.config.k8s.io/v1alpha1/Component' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenKustomizeComponent_SucceedsAndGeneratesAKustomizeComponentFile()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "kustomization.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"kustomize component --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen native' command succeeds and returns the help text.
  /// </summary>
  [Fact]
  public async Task KSailGenNative_SucceedsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailGenCommand(console);

    //Act
    int exitCode = await ksailCommand.InvokeAsync("native", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the 'ksail gen native cluster' command succeeds and returns the help text.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeCluster_SucceedsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailGenCommand(console);

    //Act
    int exitCode = await ksailCommand.InvokeAsync("native cluster", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the 'ksail gen native cluster api-service' command generates an 'apiregistration.k8s.io/v1/APIService' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeClusterApiService_SucceedsAndGeneratesAnAPIServiceResource()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "api-service.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"native cluster api-service --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen native cluster cluster-role-binding' command generates a 'rbac.authorization.k8s.io/v1/ClusterRoleBinding' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeClusterClusterRoleBinding_SucceedsAndGeneratesAClusterRoleBindingResource()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "cluster-role-binding.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"native cluster cluster-role-binding --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen native cluster cluster-role' command generates a 'rbac.authorization.k8s.io/v1/ClusterRole' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeClusterClusterRole_SucceedsAndGeneratesAClusterRoleResource()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "cluster-role.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"native cluster cluster-role --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen native cluster flow-schema' command generates a 'flowcontrol.apiserver.k8s.io/v1/FlowSchema' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeClusterFlowSchema_SucceedsAndGeneratesAFlowSchemaResource()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "flow-schema.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"native cluster flow-schema --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen native cluster namespace' command generates a 'core/v1/Namespace' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeClusterNamespace_SucceedsAndGeneratesANamespaceResource()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "namespace.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"native cluster namespace --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen native cluster network-policy' command generates a 'networking.k8s.io/v1/NetworkPolicy' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeClusterNetworkPolicy_SucceedsAndGeneratesANetworkPolicyResource()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "network-policy.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"native cluster network-policy --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen native cluster persistent-volume' command generates a 'core/v1/PersistentVolume' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeClusterPersistentVolume_SucceedsAndGeneratesAPersistentVolumeResource()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "persistent-volume.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"native cluster persistent-volume --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen native cluster priority-level-configuration' command generates a 'flowcontrol.apiserver.k8s.io/v1/PriorityLevelConfiguration' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeClusterPriorityLevelConfiguration_SucceedsAndGeneratesAPriorityLevelConfigurationResource()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "priority-level-configuration.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"native cluster priority-level-configuration --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen native cluster resource-quota' command generates a 'core/v1/ResourceQuota' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeClusterResourceQuota_SucceedsAndGeneratesAResourceQuotaResource()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "resource-quota.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"native cluster resource-quota --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen native cluster role-binding' command generates a 'rbac.authorization.k8s.io/v1/RoleBinding' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeClusterRoleBinding_SucceedsAndGeneratesARoleBindingResource()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "role-binding.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"native cluster role-binding --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen native cluster role' command generates a 'rbac.authorization.k8s.io/v1/Role' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeClusterRole_SucceedsAndGeneratesARoleResource()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "role.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"native cluster role --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen native cluster runtime-class' command generates a 'node.k8s.io/v1/RuntimeClass' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeClusterRuntimeClass_SucceedsAndGeneratesARuntimeClassResource()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "runtime-class.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"native cluster runtime-class --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen native cluster service-account' command generates a 'core/v1/ServiceAccount' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeClusterServiceAccount_SucceedsAndGeneratesAServiceAccountResource()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "service-account.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"native cluster service-account --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen native cluster storage-version-migration' command generates a 'storagemigration.k8s.io/v1alpha1/StorageVersionMigration' resource.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeClusterStorageVersionMigration_SucceedsAndGeneratesAStorageVersionMigrationResource()
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), "storage-version-migration.yaml");
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync($"native cluster storage-version-migration --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents);

    //Cleanup
    File.Delete(outputPath);
  }

  /// <summary>
  /// Tests that the 'ksail gen native config-and-storage' command succeeds and returns the help text.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeConfigAndStorage_SucceedsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailGenCommand(console);

    //Act
    int exitCode = await ksailCommand.InvokeAsync("native config-and-storage", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  // TODO: Add tests for the remaining 'ksail gen native config-and-storage' commands.

  /// <summary>
  /// Tests that the 'ksail gen native metadata' command succeeds and returns the help text.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeMetadata_SucceedsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailGenCommand(console);

    //Act
    int exitCode = await ksailCommand.InvokeAsync("native metadata", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  // TODO: Add tests for the remaining 'ksail gen native metadata' commands.

  /// <summary>
  /// Tests that the 'ksail gen native service' command succeeds and returns the help text.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeService_SucceedsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailGenCommand(console);

    //Act
    int exitCode = await ksailCommand.InvokeAsync("native service", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  // TODO: Add tests for the remaining 'ksail gen native service' commands.

  /// <summary>
  /// Tests that the 'ksail gen native workloads' command succeeds and returns the help text.
  /// </summary>
  [Fact]
  public async Task KSailGenNativeWorkloads_SucceedsAndPrintsHelp()
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailGenCommand(console);

    //Act
    int exitCode = await ksailCommand.InvokeAsync("native workloads", console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  // TODO: Add tests for the remaining 'ksail gen native workloads' commands.
}
