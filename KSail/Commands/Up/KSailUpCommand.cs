using System.CommandLine;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Options;

namespace KSail.Commands.Up;

sealed class KSailUpCommand : Command
{
  readonly NameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ConfigOption _configOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly ManifestsOption _manifestsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly KustomizationsOption _kustomizationsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly TimeoutOption _timeoutOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly SOPSOption _sopsOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly LintOption _lintOption = new() { Arity = ArgumentArity.ZeroOrOne };
  internal KSailUpCommand() : base("up", "Provision a cluster")
  {
    AddOption(_nameOption);
    AddOption(_configOption);
    AddOption(_manifestsOption);
    AddOption(_kustomizationsOption);
    AddOption(_timeoutOption);
    AddOption(_sopsOption);
    AddOption(_lintOption);

    AddValidator(result =>
    {
      string? name = result.GetValueForOption(_nameOption);
      if (string.IsNullOrEmpty(name))
      {
        result.ErrorMessage = "Required argument 'ClusterName' missing for command: 'up'.";
        return;
      }
      string? configPath = $"{name}-{result.GetValueForOption(_configOption)}";
      string? manifestsPath = result.GetValueForOption(_manifestsOption);
      if (string.IsNullOrEmpty(configPath) || !File.Exists(configPath))
      {
        result.ErrorMessage = $"Config file '{configPath}' does not exist";
      }
      else if (string.IsNullOrEmpty(manifestsPath) || !Directory.Exists(manifestsPath))
      {
        result.ErrorMessage = $"Manifests directory '{manifestsPath}' does not exist";
      }
    });
    this.SetHandler(async (context) =>
    {
      var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
      config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));
      config.UpdateConfig("Spec.ConfigPath", context.ParseResult.GetValueForOption(_configOption));
      config.UpdateConfig("Spec.ManifestsDirectory", context.ParseResult.GetValueForOption(_manifestsOption));
      config.UpdateConfig("Spec.KustomizationDirectory", context.ParseResult.GetValueForOption(_kustomizationsOption));
      config.UpdateConfig("Spec.Timeout", context.ParseResult.GetValueForOption(_timeoutOption));
      config.UpdateConfig("Spec.Sops", context.ParseResult.GetValueForOption(_sopsOption));
      config.UpdateConfig("Spec.UpOptions.Lint", context.ParseResult.GetValueForOption(_lintOption));

      var handler = new KSailUpCommandHandler(config);
      try
      {
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
