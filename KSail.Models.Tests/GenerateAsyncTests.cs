using Devantler.K9sCLI;
using KSail.Models.CLI;
using KSail.Models.CLI.Commands;
using KSail.Models.CLI.Commands.Init;
using KSail.Models.CLI.Commands.Sops;
using KSail.Models.Registry;

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
  public async Task InitializeKSailCluster_WithNoProperties_ShouldReturnDefaultValues()
  {
    // Arrange
    var cluster = new KSailCluster();

    // Act

    // Assert
    _ = await Verify(cluster);
  }

  /// <summary>
  /// Tests that <see cref="KSailCluster"/> object is initialized with specified values.
  /// </summary>
  [Fact]
  public async Task InitializeKSailCluster_WithSpecifiedProperties_ShouldReturnCustomValues()
  {
    // Arrange
    var cluster = new KSailCluster("my-cluster")
    {
      Spec = new KSailClusterSpec("my-cluster")
      {
        Connection = new KSailConnectionOptions
        {
          Kubeconfig = "./.kube/config",
          Context = "my-cluster",
          Timeout = "5m",
        },
        Project = new KSailProjectOptions
        {
          ManifestsDirectory = "./k8s",
          KustomizationDirectory = "./clusters/my-cluster/flux-system",
          ConfigPath = "./k3d-config.yaml",
          Distribution = KSailKubernetesDistribution.K3d,
          GitOpsTool = KSailGitOpsTool.Flux,
          ContainerEngine = KSailContainerEngine.Docker,
          Sops = true
        },
        Registries =
        [
          new KSailRegistry
          {
            Name = "ksail-registry",
            HostPort = 5000,
            IsGitOpsOCISource = true,
          },
          new KSailRegistry
          {
            Name = "mirror-docker.io",
            Proxy = new KSailRegistryProxy
            {
              Url = new Uri("https://registry-1.docker.io")
            },
            HostPort = 5001
          },
          new KSailRegistry
          {
            Name = "mirror-registry.k8s.io",
            Proxy = new KSailRegistryProxy
            {
              Url = new Uri("https://registry.k8s.io")
            },
            HostPort = 5002
          },
          new KSailRegistry
          {
            Name = "mirror-gcr.io",
            Proxy = new KSailRegistryProxy
            {
              Url = new Uri("https://gcr.io")
            },
            HostPort = 5002
          },
          new KSailRegistry
          {
            Name = "mirror-ghcr.io",
            Proxy = new KSailRegistryProxy
            {
              Url = new Uri("https://ghcr.io")
            },
            HostPort = 5003
          },
          new KSailRegistry
          {
            Name = "mirror-mcr.microsoft.com",
            Proxy = new KSailRegistryProxy
            {
              Url = new Uri("https://mcr.microsoft.com")
            },
            HostPort = 5004
          },
          new KSailRegistry
          {
            Name = "mirror-quay.io",
            Proxy = new KSailRegistryProxy
            {
              Url = new Uri("https://quay.io")
            },
            HostPort = 5005
          }
        ],
        CLI = new KSailCLIOptions
        {
          CheckOptions = new KSailCLICheckOptions
          {
          },
          DebugOptions = new KSailCLIDebugOptions
          {
            Editor = Editor.Nano
          },
          DownOptions = new KSailCLIDownOptions
          {
            Registries = true
          },
          GenOptions = new KSailCLIGenOptions
          {
          },
          InitOptions = new KSailCLIInitOptions
          {
            Template = KSailCLIInitTemplate.Simple,
            Components = true,
            HelmReleases = true,
            PostBuildVariables = true
          },
          LintOptions = new KSailCLILintOptions
          {
          },
          ListOptions = new KSailCLIListOptions
          {
          },
          SopsOptions = new KSailCLISopsOptions
          {
          },
          StartOptions = new KSailCLIStartOptions
          {
          },
          StopOptions = new KSailCLIStopOptions
          {
          },
          UpOptions = new KSailCLIUpOptions
          {
            Lint = true,
            Reconcile = true,
          },
          UpdateOptions = new KSailCLIUpdateOptions
          {
            Lint = true,
            Reconcile = true,
          }

        }
      }
    };

    // Act

    // Assert
    _ = await Verify(cluster);
  }
}
