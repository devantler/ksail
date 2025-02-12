using System.CommandLine;
using KSail.Commands.Secrets.Commands;

namespace KSail.Commands.Secrets;

sealed class KSailSecretsCommand : Command
{
  internal KSailSecretsCommand(IConsole? console = default) : base("secrets", "Manage secrets")
  {
    AddCommands();
    this.SetHandler(async (context) =>
      {
        context.ExitCode = await this.InvokeAsync("--help", console).ConfigureAwait(false);
      }
    );
  }

  void AddCommands()
  {
    AddCommand(new KSailSecretsGenerateCommand());
    AddCommand(new KSailSecretsDeleteCommand());
    AddCommand(new KSailSecretsListCommand());
  }
}
