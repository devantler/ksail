
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native;
using KSail.Commands.Gen.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Native;

class KSailGenNativeClusterTrustBundleCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly FileOutputOption _outputOption = new("./cluster-trust-bundle.yaml");
  readonly KSailGenNativeClusterTrustBundleCommandHandler _handler = new();
  public KSailGenNativeClusterTrustBundleCommand() : base("cluster-trust-bundle", "Generate a 'certificates.k8s.io/v1alpha1/ClusterTrustBundle' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        string outputFile = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        try
        {
          Console.WriteLine($"✚ generating {outputFile}");
          context.ExitCode = await _handler.HandleAsync(outputFile, context.GetCancellationToken()).ConfigureAwait(false);
        }
        catch (OperationCanceledException ex)
        {
          _ = _exceptionHandler.HandleException(ex);
          context.ExitCode = 1;
        }
      }
    );
  }
}
