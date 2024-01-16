using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Down.Handlers;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Models;
using KSail.Options;
using KSail.Provisioners;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KSail.Commands.Up;

internal sealed class KSailUpCommand : Command
{
  private readonly NameArgument nameArgument = new() { Arity = ArgumentArity.ZeroOrOne };
  private readonly ConfigOption configOption = new() { IsRequired = true };
  private readonly ManifestsOption manifestsOption = new();
  private readonly KustomizationsOption kustomizationsOption = new();
  private readonly TimeoutOption timeoutOption = new();
  private readonly NoSOPSOption noSOPSOption = new();
  private readonly NoGitOpsOption noGitOpsOption = new();
  private static readonly DockerProvisioner dockerProvisioner = new();

  private static readonly IDeserializer yamlDeserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .IgnoreUnmatchedProperties()
    .Build();
  internal KSailUpCommand() : base("up", "Create a K8s cluster")
  {
    AddArgument(nameArgument);
    AddOption(configOption);
    AddOption(manifestsOption);
    AddOption(kustomizationsOption);
    AddOption(timeoutOption);
    AddOption(noSOPSOption);
    AddOption(noGitOpsOption);

    this.SetHandler(async (name, configPath, manifestsPath, kustomizationsPath, timeout, noSOPS, noGitOps) =>
    {
      var config = yamlDeserializer.Deserialize<K3dConfig>(File.ReadAllText(configPath));
      if (string.IsNullOrEmpty(name))
      {
        name = config?.Metadata.Name ?? name;
      }
      await dockerProvisioner.CheckReadyAsync();

      if (await K3dProvisioner.ExistsAsync(name))
      {
        await KSailDownCommandHandler.HandleAsync(name);
      }

      Console.WriteLine("🧮 Creating pull-through registries...");
      await dockerProvisioner.CreateRegistryAsync("proxy-docker.io", 5001, new Uri("https://registry-1.docker.io"));
      await dockerProvisioner.CreateRegistryAsync("proxy-registry.k8s.io", 5002, new Uri("https://registry.k8s.io"));
      await dockerProvisioner.CreateRegistryAsync("proxy-gcr.io", 5003, new Uri("https://gcr.io"));
      await dockerProvisioner.CreateRegistryAsync("proxy-ghcr.io", 5004, new Uri("https://ghcr.io"));
      await dockerProvisioner.CreateRegistryAsync("proxy-quay.io", 5005, new Uri("https://quay.io"));
      await dockerProvisioner.CreateRegistryAsync("proxy-mcr.microsoft.com", 5006, new Uri("https://mcr.microsoft.com"));
      Console.WriteLine();

      if (noGitOps)
      {
        await K3dProvisioner.ProvisionAsync(name, configPath);
      }
      else
      {
        await KSailUpGitOpsCommandHandler.HandleAsync(name, configPath, manifestsPath, kustomizationsPath, timeout, noSOPS);
      }
    }, nameArgument, configOption, manifestsOption, kustomizationsOption, timeoutOption, noSOPSOption, noGitOpsOption);
  }
}
