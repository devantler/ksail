using System.CommandLine;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Options;
using KSail.Presentation.Commands.Up.Options;

namespace KSail.Commands.Up;

sealed class KSailUpK3dCommand : Command
{
  readonly NameOption _nameOption = new("name of the cluster");
  readonly PullThroughRegistriesOption _pullThroughRegistriesOption = new();
  readonly ConfigPathOption _configPathOption = new();

  internal KSailUpK3dCommand() : base("k3d", "create a k3d cluster ")
  {
    AddGlobalOptions();
    AddCommands();

    this.SetHandler(
      KSailUpK3dCommandHandler.Handle, _nameOption, _pullThroughRegistriesOption, _configPathOption
    );
  }

  void AddGlobalOptions()
  {
    AddGlobalOption(_nameOption);
    AddGlobalOption(_pullThroughRegistriesOption);
    AddGlobalOption(_configPathOption);
  }

  void AddCommands() => AddCommand(new KSailUpK3dFluxCommand(_nameOption, _pullThroughRegistriesOption, _configPathOption));
}
