using System.CommandLine;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Models.K3d;
using KSail.Options;
using YamlDotNet.Serialization;

namespace KSail.Commands.Up;

sealed class KSailUpK3dCommand : Command
{
  readonly NameOption _nameOption = new("name of the cluster");
  readonly PullThroughRegistriesOption _pullThroughRegistriesOption = new() { IsRequired = true };
  readonly ConfigPathOption _configPathOption = new();
  static readonly Deserializer _yamlDeserializer = new();

  internal KSailUpK3dCommand() : base("k3d", "create a k3d cluster ")
  {
    AddGlobalOptions();
    AddCommands();

    this.SetHandler(async (name, pullThroughRegistries, configPath) =>
    {
      var config = string.IsNullOrEmpty(configPath) ? null : _yamlDeserializer.Deserialize<K3dConfig>(File.ReadAllText(configPath));
      name = config?.Metadata.Name ?? name;
      await KSailUpK3dCommandHandler.HandleAsync(name, pullThroughRegistries, configPath);
    }, _nameOption, _pullThroughRegistriesOption, _configPathOption);
  }

  void AddGlobalOptions()
  {
    AddGlobalOption(_nameOption);
    AddGlobalOption(_pullThroughRegistriesOption);
    AddGlobalOption(_configPathOption);
  }

  void AddCommands() => AddCommand(new KSailUpK3dFluxCommand(_nameOption, _pullThroughRegistriesOption, _configPathOption));
}
