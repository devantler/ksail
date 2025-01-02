using System.CommandLine;
using Devantler.KubernetesValidator.ClientSide.Schemas;
using Devantler.KubernetesValidator.ClientSide.YamlSyntax;
using KSail.Commands.Lint.Handlers;
using KSail.Options;
using KSail.Utils;
using YamlDotNet.Core;

namespace KSail.Commands.Lint;

sealed class KSailLintCommand : Command
{
  readonly ExceptionHandler _exceptionHandler = new();
  readonly NameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly PathOption _workingDirectoryOption = new("The directory where the manifest files are located") { Arity = ArgumentArity.ZeroOrOne };
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
      catch (YamlException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (YamlSyntaxValidatorException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (SchemaValidatorException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (OperationCanceledException ex)
      {
        _ = _exceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }
}
