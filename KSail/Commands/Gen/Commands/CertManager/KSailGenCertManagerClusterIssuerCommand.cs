
using System.CommandLine;
using KSail.Commands.Gen.Handlers.CertManager;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.CertManager;

class KSailGenCertManagerClusterIssuerCommand : Command
{
  readonly FileOutputOption _outputOption = new("./cluster-issuer.yaml");

  public KSailGenCertManagerClusterIssuerCommand() : base("cluster-issuer", "Generate a 'cert-manager.io/v1/ClusterIssuer' resource.")
  {
    AddOption(_outputOption);

    this.SetHandler(async (context) =>
      {
        string outputPath = context.ParseResult.RootCommandResult.GetValueForOption(_outputOption)!;
        var handler = new KSailGenCertManagerClusterIssuerCommandHandler();
        try
        {
          context.ExitCode = await handler.HandleAsync(outputPath, context.GetCancellationToken()).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
          context.ExitCode = 1;
        }
      }
    );
  }
}
