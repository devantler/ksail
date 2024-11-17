using System.CommandLine;
using KSail.Commands.SOPS.Commands;

namespace KSail.Commands.SOPS;

sealed class KSailSOPSCommand : Command
{
  internal KSailSOPSCommand() : base("sops", "Manage secrets in Git")
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
    AddCommand(new KSailSOPSGenCommand());
    AddCommand(new KSailSOPSListCommand());
    AddCommand(new KSailSOPSEditCommand());
    AddCommand(new KSailSOPSEncryptCommand());
    AddCommand(new KSailSOPSDecryptCommand());
    AddCommand(new KSailSOPSImportCommand());
    AddCommand(new KSailSOPSExportCommand());
  }
}
