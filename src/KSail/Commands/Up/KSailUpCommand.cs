using System.CommandLine;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Models;
using KSail.Options;
using YamlDotNet.Serialization;

namespace KSail.Commands.Up;

sealed class KSailUpCommand : Command
{
  readonly NameOption nameOption = new("Name of the cluster");
  readonly PullThroughRegistriesOption pullThroughRegistriesOption = new() { IsRequired = true };
  readonly ConfigPathOption configPathOption = new();
  static readonly Deserializer yamlDeserializer = new();
  internal KSailUpCommand() : base("up", "Create a K8s cluster")
  {
    AddGlobalOptions();
    AddCommands();

    this.SetHandler(async (name, pullThroughRegistries, configPath) =>
    {
      var config = string.IsNullOrEmpty(configPath) ? null : yamlDeserializer.Deserialize<K3dConfig>(File.ReadAllText(configPath));
      name = config?.Metadata.Name ?? name;
      await KSailUpCommandHandler.HandleAsync(name, pullThroughRegistries, configPath);
    }, nameOption, pullThroughRegistriesOption, configPathOption);
  }
  void AddGlobalOptions()
  {
    AddGlobalOption(nameOption);
    AddGlobalOption(pullThroughRegistriesOption);
    AddGlobalOption(configPathOption);
  }

  void AddCommands() => AddCommand(new KSailUpGitOpsCommand(nameOption, pullThroughRegistriesOption, configPathOption));
}
