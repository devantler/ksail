using System.CommandLine;
using KSail.Provisioners;

namespace KSail.Commands.Down.Handlers;

class KSailDownCommandHandler(IConsole console)
{
  readonly IConsole console = console;
  internal async Task HandleAsync(string name)
  {
    await K3dProvisioner.DeprovisionAsync(name);
    console.WriteLine("");
  }
}
