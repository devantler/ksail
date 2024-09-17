
using System.CommandLine;
using KSail.Commands.Gen.Handlers.CertManager;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.CertManager;

class KSailGenCertManagerCertificateCommand : Command
{
  readonly FileOutputOption _outputOption = new("./certificate.yaml");
  readonly KSailGenCertManagerCertificateCommandHandler _handler = new();
  public KSailGenCertManagerCertificateCommand() : base("certificate", "Generate a 'cert-manager.io/v1/Certificate' resource.")
  {
    AddOption(_outputOption);

    this.SetHandler(async (context) =>
      {
        string outputPath = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        await _handler.HandleAsync(outputPath, context.GetCancellationToken()).ConfigureAwait(false);
      }
    );
  }
}
