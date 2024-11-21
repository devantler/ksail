
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.CertManager;

class KSailGenCertManagerCommand : Command
{
  public KSailGenCertManagerCommand(IConsole? console = default) : base("cert-manager", "Generate a CertManager resource.")
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
    AddCommand(new KSailGenCertManagerCertificateCommand());
    AddCommand(new KSailGenCertManagerClusterIssuerCommand());
  }
}
