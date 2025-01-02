
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native;
using KSail.Commands.Gen.Options;
using KSail.Utils;

namespace KSail.Commands.Gen.Commands.Native;

class KSailGenNativeRoleCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly FileOutputOption _outputOption = new("./role.yaml");
  readonly KSailGenNativeRoleCommandHandler _handler = new();
  public KSailGenNativeRoleCommand() : base("role", "Generate a 'rbac.authorization.k8s.io/v1/Role' resource.")
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
