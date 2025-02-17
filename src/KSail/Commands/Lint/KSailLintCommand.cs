using System.CommandLine;
using KSail.Commands.Lint.Handlers;
using KSail.Options;
using KSail.Utils;

namespace KSail.Commands.Lint;

sealed class KSailLintCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly MetadataNameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly PathOption _workingDirectoryOption = new("Path to the working directory for your project") { Arity = ArgumentArity.ZeroOrOne };
  internal KSailLintCommand() : base(
   "lint", "Lint manifests for a cluster"
  )
  {
    AddOption(_nameOption);
    AddOption(_workingDirectoryOption);
    this.SetHandler(async (context) =>
    {
      try
      {
        string workingDirectory = context.ParseResult.GetValueForOption(_workingDirectoryOption) ?? Environment.CurrentDirectory;
        string? name = context.ParseResult.GetValueForOption(_nameOption);

        var config = await KSailClusterConfigLoader.LoadAsync(workingDirectory, name).ConfigureAwait(false);
        config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));
        config.UpdateConfig("Spec.Project.WorkingDirectory", workingDirectory);

        Console.WriteLine("🧹 Linting manifest files");
        var handler = new KSailLintCommandHandler();
        context.ExitCode = await handler.HandleAsync(config, context.GetCancellationToken()).ConfigureAwait(false) ? 0 : 1;
        Console.WriteLine();
      }
      catch (Exception ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }
}
