using System.CommandLine;
using KSail.Commands.Lint.Handlers;
using KSail.Options;

namespace KSail.Commands.Lint;

sealed class KSailLintCommand : Command
{
  readonly NameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly PathOption _manifestsPathOption = new("./k8s", " Path to the manifests directory") { Arity = ArgumentArity.ZeroOrOne };
  internal KSailLintCommand() : base(
   "lint", "Lint manifests for a cluster"
  )
  {
    AddOption(_nameOption);
    AddOption(_manifestsPathOption);
    this.SetHandler(async (context) =>
    {
      var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
      config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));
      config.UpdateConfig("Spec.ManifestsDirectory", context.ParseResult.GetValueForOption(_manifestsPathOption));
      try
      {
        Console.WriteLine("🧹 Linting manifest files");
        context.ExitCode = await KSailLintCommandHandler.HandleAsync(config, context.GetCancellationToken()).ConfigureAwait(false);
        Console.WriteLine("");
      }
      catch (OperationCanceledException)
      {
        Console.WriteLine("✕ Operation was canceled by the user.");
        context.ExitCode = 1;
      }
    });
  }
}
