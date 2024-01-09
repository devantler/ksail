using System.CommandLine;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Models.K3d;
using KSail.Options;
using YamlDotNet.Serialization;

namespace KSail.Commands.Up;

sealed class KSailUpK3dCommand : Command
{
  readonly NameOption nameOption = new("name of the cluster");
  readonly PullThroughRegistriesOption pullThroughRegistriesOption = new() { IsRequired = true };
  readonly ConfigPathOption configPathOption = new();
  static readonly Deserializer yamlDeserializer = new();

  internal KSailUpK3dCommand() : base("k3d", "create a k3d cluster ")
  {
    AddGlobalOptions();
    AddCommands();

    this.SetHandler(async (name, pullThroughRegistries, configPath) =>
    {
      var config = string.IsNullOrEmpty(configPath) ? null : yamlDeserializer.Deserialize<K3dConfig>(File.ReadAllText(configPath));
      name = config?.Metadata.Name ?? name;
      await KSailUpK3dCommandHandler.HandleAsync(name, pullThroughRegistries, configPath);
    }, nameOption, pullThroughRegistriesOption, configPathOption);
  }

  void AddGlobalOptions()
  {
    AddGlobalOption(nameOption);
    AddGlobalOption(pullThroughRegistriesOption);
    AddGlobalOption(configPathOption);
  }

  void AddCommands() => AddCommand(new KSailUpK3dFluxCommand(nameOption, pullThroughRegistriesOption, configPathOption));
}
