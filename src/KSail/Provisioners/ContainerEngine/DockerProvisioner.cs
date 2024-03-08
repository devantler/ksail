using System.Runtime.InteropServices;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace KSail.Provisioners.ContainerEngine;

sealed class DockerProvisioner : IContainerEngineProvisioner
{
  readonly DockerClient _dockerClient;

  DockerProvisioner()
  {
    string? dockerHost = Environment.GetEnvironmentVariable("DOCKER_HOST");
    if (!string.IsNullOrEmpty(dockerHost))
    {
      var uri = new Uri(dockerHost);
      _dockerClient = new DockerClientConfiguration(uri).CreateClient();
      return;
    }
    _dockerClient = new DockerClientConfiguration().CreateClient();
  }

  public async Task<int> CheckReadyAsync(CancellationToken token)
  {
    try
    {
      await _dockerClient.System.PingAsync(token);
    }
    catch (Exception e)
    {
      Console.WriteLine($"✕ Could not connect to the default Docker Socket: {e.Message}");
      if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
      {
        Console.WriteLine("  Have you enabled the option 'Allow the default Docker socket to be used' in Docker Desktop?");
        Console.WriteLine("  You can find this option in the Docker Desktop GUI under 'Preferences > Advanced'");
      }
      return 1;
    }
    return 0;
  }

  public async Task<int> CreateRegistryAsync(string name, int port, CancellationToken token, Uri? proxyUrl = null)
  {
    if (proxyUrl != null)
    {
      Console.WriteLine($"► Creating pull-through registry '{name}' on port '{port}' for '{proxyUrl}'");
    }
    else
    {
      Console.WriteLine($"► Creating registry '{name}' on port '{port}'");
    }
    var (ExitCode, RegistryExists) = await GetContainerIdAsync(name, token);
    if (ExitCode != 0)
    {
      return 1;
    }

    if (RegistryExists != null)
    {
      Console.WriteLine($"✔ Registry '{name}' already exists. Skipping");
      return 0;
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
            ["5000/tcp"] =
          [
            new() {
              HostPort = $"{port}"
            }
          ]
          },
          RestartPolicy = new RestartPolicy
          {
            Name = RestartPolicyKind.Always
          },
          Binds =
          [
            $"{name}:/var/lib/registry"
          ]
        },
        Env = proxyUrl != null ? new List<string>
      {
        $"REGISTRY_PROXY_REMOTEURL={proxyUrl}"
      } : null
      });
      _ = await _dockerClient.Containers.StartContainerAsync(registry.ID, new ContainerStartParameters());
    }
    catch (DockerApiException)
    {
      Console.WriteLine($"✕ Could not create registry '{name}'");
      return 1;
    }
    return 0;
  }
  public async Task<int> DeleteRegistryAsync(string name, CancellationToken token)
  {
    var (ExitCode, ContainerId) = await GetContainerIdAsync(name, token);
    if (ExitCode != 0)
    {
      return 1;
    }

    if (string.IsNullOrEmpty(ContainerId))
    {
      Console.WriteLine($"✕ Could not find registry '{name}'. Skipping");
    }
    else
    {
      _ = await _dockerClient.Containers.StopContainerAsync(ContainerId, new ContainerStopParameters(), token);
      await _dockerClient.Containers.RemoveContainerAsync(ContainerId, new ContainerRemoveParameters(), token);
    }
    return 0;
  }

  public async Task<(int ExitCode, string? Result)> GetContainerIdAsync(string name, CancellationToken token)
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
    }, token);

    return (0, containers.FirstOrDefault()?.ID);
  }
}
