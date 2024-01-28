using Docker.DotNet;
using Docker.DotNet.Models;
using KSail.Enums;
using KSail.Exceptions;

namespace KSail.Services.Provisioners.ContainerEngine;

sealed class DockerProvisioner : IContainerEngineProvisioner
{
  readonly DockerClient _dockerClient = new DockerClientConfiguration(
    new Uri("unix:///var/run/docker.sock")
  ).CreateClient();

  public async Task CheckReadyAsync()
  {
    Console.WriteLine("🐳 Checking Docker is running...");
    try
    {
      await _dockerClient.System.PingAsync();
    }
    catch (Exception)
    {
      throw new KSailException("✕ Could not connect to Docker. Is Docker running?");
    }
    Console.WriteLine("✔ Docker is running...");
    Console.WriteLine();
  }

  public async Task CreateRegistryAsync(string name, int port, Uri? proxyUrl = null)
  {
    if (proxyUrl != null)
    {
      Console.WriteLine($"► Creating pull-through registry '{name}' on port '{port}' for '{proxyUrl}'...");
    }
    else
    {
      Console.WriteLine($"► Creating registry '{name}' on port '{port}'...");
    }
    bool registryExists = await GetContainerIdAsync(name) != null;

    if (registryExists)
    {
      Console.WriteLine($"✔ Registry '{name}' already exists. Skipping...");
      return;
    }
    CreateContainerResponse registry;
    try
    {
      await _dockerClient.Images.CreateImageAsync(new ImagesCreateParameters
      {
        FromImage = "registry:2"
      }, null, new Progress<JSONMessage>());
      registry = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
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
    }
    catch (DockerApiException e)
    {
      throw new KSailException($"✕ Could not create registry '{name}'...", e);
    }
  }
  public async Task DeleteRegistryAsync(string name)
  {
    string? containerId = await GetContainerIdAsync(name);

    if (string.IsNullOrEmpty(containerId))
    {
      Console.WriteLine($"✕ Could not find registry '{name}'. Skipping...");
    }
    else
    {
      _ = await _dockerClient.Containers.StopContainerAsync(containerId, new ContainerStopParameters());
      await _dockerClient.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters());
    }
  }

  public Task<ContainerEngineType> GetContainerEngineTypeAsync() => Task.FromResult(ContainerEngineType.Docker);

  public async Task<string?> GetContainerIdAsync(string name)
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
