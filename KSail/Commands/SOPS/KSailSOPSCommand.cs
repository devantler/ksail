using System.CommandLine;
using KSail.Commands.Sops.Commands;

namespace KSail.Commands.Sops;

sealed class KSailSopsCommand : Command
{
  internal KSailSopsCommand() : base("sops", "Manage secrets in Git")
  {
    AddCommands();

    this.SetHandler(async (context) =>
      {
        context.ExitCode = await this.InvokeAsync("--help").ConfigureAwait(false);
      }
    );
  }

  void AddCommands()
  {
    AddCommand(new KSailSopsGenCommand());
    AddCommand(new KSailSopsListCommand());
    AddCommand(new KSailSopsEditCommand());
    AddCommand(new KSailSopsEncryptCommand());
    AddCommand(new KSailSopsDecryptCommand());
    AddCommand(new KSailSopsImportCommand());
    AddCommand(new KSailSopsExportCommand());
  }
}
