
using Docker.DotNet;
using Docker.DotNet.Models;

namespace KSail.Provisioners.ContainerOrchestrator;

sealed class DockerProvisioner : IContainerOrchestratorProvisioner
{
  readonly DockerClient _dockerClient = new DockerClientConfiguration(
    new Uri("unix:///var/run/docker.sock")
  ).CreateClient();
  internal async Task CreateRegistryAsync(string name, int port, Uri? proxyUrl = null)
  {
    bool registryExists = await GetContainerId(name) != null;

    if (registryExists)
    {
      Console.WriteLine($"🧮✅ Registry '{name}' already exists. Skipping...");
      return;
    }
    var registry = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
    {
      Image = "registry:2",
      Name = name,
      HostConfig = new HostConfig
      {
        PortBindings = new Dictionary<string, IList<PortBinding>>
        {
          ["5000/tcp"] = new List<PortBinding>
          {
            new() {
              HostPort = $"{port}"
            }
          }
        },
        RestartPolicy = new RestartPolicy
        {
          Name = RestartPolicyKind.Always
        },
        Binds = new List<string>
        {
          $"{name}:/var/lib/registry"
        }
      },
      Env = proxyUrl != null ? new List<string>
      {
        $"REGISTRY_PROXY_REMOTEURL={proxyUrl}"
      } : null
    });
    _ = await _dockerClient.Containers.StartContainerAsync(registry.ID, new ContainerStartParameters());
    Console.WriteLine($"🧮✅ Registry '{name}' created successfully.");
  }

  internal async Task DeleteRegistryAsync(string name)
  {
    string? containerId = await GetContainerId(name);

    if (containerId != null)
    {
      _ = await _dockerClient.Containers.StopContainerAsync(containerId, new ContainerStopParameters());
      await _dockerClient.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters());
    }
  }

  async Task<string?> GetContainerId(string name)
  {
    var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters
    {
      All = true,
      Filters = new Dictionary<string, IDictionary<string, bool>>
      {
        ["name"] = new Dictionary<string, bool>
        {
          [name] = true
        }
      }
    });

    return containers.FirstOrDefault()?.ID;
  }
}
