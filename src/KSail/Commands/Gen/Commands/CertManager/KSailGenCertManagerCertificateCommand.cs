
using System.CommandLine;
using KSail.Commands.Gen.Handlers.CertManager;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.CertManager;

class KSailGenCertManagerCertificateCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly GenericPathOption _outputOption = new("./certificate.yaml");
  public KSailGenCertManagerCertificateCommand() : base("certificate", "Generate a 'cert-manager.io/v1/Certificate' resource.")
  {
    AddOption(_outputOption);

    this.SetHandler(async (context) =>
      {
        try
        {
          string outputFile = context.ParseResult.CommandResult.GetValueForOption(_outputOption) ?? "./certificate.yaml";
          bool overwrite = context.ParseResult.CommandResult.GetValueForOption(CLIOptions.Generator.OverwriteOption) ?? false;
          Console.WriteLine($"✚ generating {outputFile}");
          var handler = new KSailGenCertManagerCertificateCommandHandler(outputFile, overwrite);
          context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
          _ = _exceptionHandler.HandleException(ex);
          context.ExitCode = 1;
        }
      }
    );
  }
}
