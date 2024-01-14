using System.CommandLine;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Models;
using KSail.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KSail.Commands.Up;

sealed class KSailUpCommand : Command
{
  readonly NameOption nameOption = new("Name of the cluster");
  readonly ConfigOption configOption = new();
  readonly PullThroughRegistriesOption pullThroughRegistriesOption = new() { IsRequired = true };
  static readonly IDeserializer yamlDeserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .IgnoreUnmatchedProperties()
    .Build();
  internal KSailUpCommand() : base("up", "Create a K8s cluster")
  {
    AddGlobalOptions();
    AddCommands();

    this.SetHandler(async (name, configPath, pullThroughRegistries) =>
    {
      //IgnoreUnma
      var config = string.IsNullOrEmpty(configPath) ? null : yamlDeserializer.Deserialize<K3dConfig>(File.ReadAllText(configPath));
      name = config?.Metadata.Name ?? name;
      await KSailUpCommandHandler.HandleAsync(name, configPath, pullThroughRegistries);
    }, nameOption, configOption, pullThroughRegistriesOption);
  }
  void AddGlobalOptions()
  {
    AddGlobalOption(nameOption);
    AddGlobalOption(pullThroughRegistriesOption);
    AddGlobalOption(configOption);
  }

  void AddCommands() => AddCommand(new KSailUpGitOpsCommand(nameOption, pullThroughRegistriesOption, configOption));
}
