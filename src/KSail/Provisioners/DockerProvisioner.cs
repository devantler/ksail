
using Docker.DotNet;
using Docker.DotNet.Models;

namespace KSail.Provisioners;

sealed class DockerProvisioner : IProvisioner
{
  readonly DockerClient dockerClient = new DockerClientConfiguration(
    new Uri("unix:///var/run/docker.sock")
  ).CreateClient();

  internal async Task CheckReadyAsync()
  {
    Console.WriteLine("🐳 Checking Docker is running...");
    try
    {
      await dockerClient.System.PingAsync();
    }
    catch (Exception)
    {
      Console.WriteLine("✕ Could not connect to Docker. Is Docker running?");
      Environment.Exit(1);
    }
    Console.WriteLine("✔ Docker is running...");
    Console.WriteLine();
  }

  internal async Task CreateRegistryAsync(string name, int port, Uri? proxyUrl = null)
  {
    if (proxyUrl != null)
    {
      Console.WriteLine($"► Creating pull-through registry '{name}' on port '{port}' for '{proxyUrl}'...");
    }
    else
    {
      Console.WriteLine($"► Creating registry '{name}' on port '{port}'...");
    }
    bool registryExists = await GetContainerId(name) != null;

    if (registryExists)
    {
      Console.WriteLine($"✔ Registry '{name}' already exists. Skipping...");
      return;
    }
    CreateContainerResponse registry;
    try
    {
      await dockerClient.Images.CreateImageAsync(new ImagesCreateParameters
      {
        FromImage = "registry:2"
      }, null, new Progress<JSONMessage>());
      registry = await dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
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
      _ = await dockerClient.Containers.StartContainerAsync(registry.ID, new ContainerStartParameters());
    }
    catch (DockerApiException e)
    {
      Console.WriteLine($" Could not create registry '{name}'. {e.Message}...");
      Environment.Exit(1);
    }
  }

  internal async Task DeleteRegistryAsync(string name)
  {
    string? containerId = await GetContainerId(name);

    if (containerId != null)
    {
      _ = await dockerClient.Containers.StopContainerAsync(containerId, new ContainerStopParameters());
      await dockerClient.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters());
    }
  }

  async Task<string?> GetContainerId(string name)
  {
    var containers = await dockerClient.Containers.ListContainersAsync(new ContainersListParameters
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
