using System.CommandLine.Parsing;
using KSail.Commands.Up.Options;
using KSail.Models;
using KSail.Options;
using YamlDotNet.Serialization;

namespace KSail.Commands.Up.Validators;

static class KSailUpGitOpsValidator
{
  static readonly Deserializer yamlDeserializer = new();
  internal static Task ValidateAsync(CommandResult commandResult, NameOption nameOption, ConfigOption configOption, ManifestsOption _manifestsOption, FluxKustomizationPathOption _fluxKustomizationPathOption)
  {
    string? name = commandResult.GetValueForOption(nameOption);
    string? configPath = commandResult.GetValueForOption(configOption);
    var config = string.IsNullOrEmpty(configPath) ? null : yamlDeserializer.Deserialize<K3dConfig>(File.ReadAllText(configPath));
    name = config?.Metadata.Name ?? name;
    if (string.IsNullOrEmpty(name))
    {
      commandResult.ErrorMessage += $"Option '{nameOption.Aliases.First()} {name ?? "null"}'. Name must be specified...{Environment.NewLine}";
      return Task.CompletedTask;
    }
    if (configPath != null && !ValidatePathExists(configPath))
    {
      commandResult.ErrorMessage += $"Invalid option '{configOption.Aliases.First()} {configPath}'. Path does not exist...{Environment.NewLine}";
      return Task.CompletedTask;
    }
    string? manifestsPath = commandResult.GetValueForOption(_manifestsOption);
    if (!ValidatePathExists(manifestsPath))
    {
      commandResult.ErrorMessage += $"Invalid option '{_manifestsOption.Aliases.First()} {manifestsPath ?? "null"}'. Path does not exist...{Environment.NewLine}";
    }
    string? fluxKustomizationPath = commandResult.GetValueForOption(_fluxKustomizationPathOption);
    fluxKustomizationPath = string.IsNullOrEmpty(fluxKustomizationPath) ? $"clusters/{name}/flux" : fluxKustomizationPath;
    string? realFluxKustomizationPath = Path.Join(manifestsPath, fluxKustomizationPath);
    if (!ValidatePathExists(realFluxKustomizationPath))
    {
      commandResult.ErrorMessage += $"Invalid option '{_fluxKustomizationPathOption.Aliases.First()} {fluxKustomizationPath ?? "null"}'. {realFluxKustomizationPath} does not exist...";
    }

    return Task.CompletedTask;
  }

  static bool ValidatePathExists(string? path) =>
    !string.IsNullOrEmpty(path) &&
    (Directory.Exists(path) ||
    File.Exists(path));
}
