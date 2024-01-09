using System.CommandLine;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Options;

namespace KSail.Commands.Up;

sealed class KSailUpK3dCommand : Command
{
  readonly NameOption _nameOption = new("name of the cluster");
  readonly PullThroughRegistriesOption _pullThroughRegistriesOption = new() { IsRequired = true };
  readonly ConfigPathOption _configPathOption = new();

  internal KSailUpK3dCommand() : base("k3d", "create a k3d cluster ")
  {
    AddGlobalOptions();
    AddCommands();

    this.SetHandler(async (name, pullThroughRegistries, configPath) =>
    {
      if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(configPath))
      {
        Console.WriteLine($"❌ Either '{_nameOption.Aliases.First()}' or '{_configPathOption.Aliases.First()}' must be specified to create a cluster.");
        Environment.Exit(1);
      }
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
