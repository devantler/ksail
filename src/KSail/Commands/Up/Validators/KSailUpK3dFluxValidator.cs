using System.CommandLine.Parsing;
using KSail.Commands.Up.Options;
using KSail.Models.K3d;
using KSail.Options;
using YamlDotNet.Serialization;

namespace KSail.Commands.Up.Validators;

static class KSailUpK3dFluxValidator
{
  static readonly Deserializer yamlDeserializer = new();
  internal static Task ValidateAsync(CommandResult commandResult, NameOption nameOption, ConfigPathOption configPathOption, ManifestsPathOption _manifestsPathOption, FluxKustomizationPathOption _fluxKustomizationPathOption)
  {
    string? name = commandResult.GetValueForOption(nameOption);
    string? configPath = commandResult.GetValueForOption(configPathOption);
    var config = string.IsNullOrEmpty(configPath) ? null : yamlDeserializer.Deserialize<K3dConfig>(File.ReadAllText(configPath));
    name = config?.Metadata.Name ?? name;
    if (string.IsNullOrEmpty(name))
    {
      commandResult.ErrorMessage += $"Option '{nameOption.Aliases.First()} {name ?? "null"}'. Name must be specified...{Environment.NewLine}";
      return Task.CompletedTask;
    }
    if (configPath != null && !ValidatePathExists(configPath))
    {
      commandResult.ErrorMessage += $"Invalid option '{configPathOption.Aliases.First()} {configPath}'. Path does not exist...{Environment.NewLine}";
      return Task.CompletedTask;
    }
    string? manifestsPath = commandResult.GetValueForOption(_manifestsPathOption);
    if (!ValidatePathExists(manifestsPath))
    {
      commandResult.ErrorMessage += $"Invalid option '{_manifestsPathOption.Aliases.First()} {manifestsPath ?? "null"}'. Path does not exist...{Environment.NewLine}";
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
