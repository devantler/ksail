using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Models;
using KSail.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KSail.Commands.Up;

sealed class KSailUpCommand : Command
{
  readonly NameArgument nameArgument = new() { Arity = ArgumentArity.ExactlyOne };
  readonly ConfigOption configOption = new() { IsRequired = true };
  readonly ManifestsOption manifestsOption = new();
  readonly KustomizationsOption kustomizationsOption = new();
  readonly TimeoutOption timeoutOption = new();
  readonly NoSOPSOption noSOPSOption = new();
  readonly NoGitOpsOption noGitOpsOption = new();
  static readonly IDeserializer yamlDeserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .IgnoreUnmatchedProperties()
    .Build();
  internal KSailUpCommand() : base("up", "Provision a K8s cluster")
  {
    AddArgument(nameArgument);
    AddOption(configOption);
    AddOption(manifestsOption);
    AddOption(kustomizationsOption);
    AddOption(timeoutOption);
    AddOption(noSOPSOption);
    AddOption(noGitOpsOption);

    AddValidator(result =>
    {
      string? name = result.GetValueForArgument(nameArgument);
      if (string.IsNullOrEmpty(name))
      {
        result.ErrorMessage = "Required argument 'Name' missing for command: 'up'.";
        return;
      }
      string? configPath = $"{name}-{result.GetValueForOption(configOption)}";
      string? manifestsPath = result.GetValueForOption(manifestsOption);
      if (string.IsNullOrEmpty(configPath) || !File.Exists(configPath))
      {
        result.ErrorMessage = $"Config file '{configPath}' does not exist";
      }
      else if (string.IsNullOrEmpty(manifestsPath) || !Directory.Exists(manifestsPath))
      {
        result.ErrorMessage = $"Manifests directory '{manifestsPath}' does not exist";
      }
    });
    this.SetHandler(async (name, configPath, manifestsPath, kustomizationsPath, timeout, noSOPS, noGitOps) =>
    {
      configPath = $"{name}-{configPath}";
      string configContent;
      K3dConfig? config = null;
      if (string.IsNullOrEmpty(configPath) || !File.Exists(configPath))
      {
        configContent = File.ReadAllText(configPath);
        config = yamlDeserializer.Deserialize<K3dConfig>(configContent);
      }
      if (string.IsNullOrEmpty(name))
      {
        name = config?.Metadata.Name ?? name;
      }
      await KSailUpCommandHandler.HandleAsync(name, configPath);
      if (!noGitOps)
      {
        await KSailUpGitOpsCommandHandler.HandleAsync(name, manifestsPath, kustomizationsPath, timeout, noSOPS);
      }
    }, nameArgument, configOption, manifestsOption, kustomizationsOption, timeoutOption, noSOPSOption, noGitOpsOption);
  }
}
