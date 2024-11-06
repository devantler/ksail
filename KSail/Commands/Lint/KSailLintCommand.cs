using System.CommandLine;
using KSail.Commands.Lint.Handlers;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Lint;

sealed class KSailLintCommand : Command
{
  readonly NameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly PathOption _manifestsPathOption = new(" Path to the manifests directory") { Arity = ArgumentArity.ZeroOrOne };
  internal KSailLintCommand() : base(
   "lint", "Lint manifests for a cluster"
  )
  {
    AddOption(_nameOption);
    AddOption(_manifestsPathOption);
    this.SetHandler(async (context) =>
    {
      try
      {
        string? manifestsPath = context.ParseResult.GetValueForOption(_manifestsPathOption);
        var config = await KSailClusterConfigLoader.LoadAsync(context.ParseResult.GetValueForOption(_manifestsPathOption), context.ParseResult.GetValueForOption(_nameOption)).ConfigureAwait(false);
        config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));
        config.UpdateConfig("Spec.ManifestsDirectory", context.ParseResult.GetValueForOption(_manifestsPathOption));

        Console.WriteLine("🧹 Linting manifest files");
        var handler = new KSailLintCommandHandler();
        context.ExitCode = await handler.HandleAsync(config, context.GetCancellationToken()).ConfigureAwait(false) ? 0 : 1;
        Console.WriteLine("");
      }
      catch (OperationCanceledException ex)
      {
        ExceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }
}
