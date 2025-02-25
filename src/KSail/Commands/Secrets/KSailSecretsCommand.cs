using System.CommandLine;
using KSail.Commands.Secrets.Commands;
using KSail.Options;

namespace KSail.Commands.Secrets;

sealed class KSailSecretsCommand : Command
{
  internal KSailSecretsCommand(IConsole? console = default) : base("secrets", "Manage secrets")
  {
    AddCommands();
    AddOptions();
    this.SetHandler(async (context) =>
      {
        context.ExitCode = await this.InvokeAsync("--help", console).ConfigureAwait(false);
      }
    );
  }

  void AddCommands()
  {
    AddCommand(new KSailSecretsEncryptCommand());
    AddCommand(new KSailSecretsDecryptCommand());
    // TODO: Include `ksail secrets edit` command when pseudo-terminal support is added to CLIWrap. See https://github.com/Tyrrrz/CliWrap/issues/225.
    //AddCommand(new KSailSecretsEditCommand());
    AddCommand(new KSailSecretsAddCommand());
    AddCommand(new KSailSecretsRemoveCommand());
    AddCommand(new KSailSecretsListCommand());
    AddCommand(new KSailSecretsImportCommand());
    AddCommand(new KSailSecretsExportCommand());
  }

  internal void AddOptions() => AddGlobalOption(CLIOptions.Project.SecretManagerOption);
}
