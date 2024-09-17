
using System.CommandLine;
using KSail.Commands.Gen.Handlers.CertManager;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.CertManager;

class KSailGenCertManagerClusterIssuerCommand : Command
{
  readonly FileOutputOption _outputOption = new("./cluster-issuer.yaml");
  readonly KSailGenCertManagerClusterIssuerCommandHandler _handler = new();
  public KSailGenCertManagerClusterIssuerCommand() : base("cluster-issuer", "Generate a 'cert-manager.io/v1/ClusterIssuer' resource.")
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
