using System.CommandLine.Parsing;
using KSail.Commands.Up.Options;
using KSail.Options;

namespace KSail.Commands.Up.Validators;

static class KSailUpGitOpsValidator
{
  internal static async Task ValidateAsync(CommandResult commandResult, NameOption nameOption, ConfigOption configOption, ManifestsOption _manifestsOption, FluxKustomizationPathOption _fluxKustomizationPathOption)
  {
    string? name = commandResult.GetValueForOption(nameOption);
    await KSailUpValidator.ValidateAsync(commandResult, nameOption, configOption);
    string? manifestsPath = commandResult.GetValueForOption(_manifestsOption);
    if (!ValidatePathExists(manifestsPath))
    {
      commandResult.ErrorMessage += $"Invalid option '{_manifestsOption.Aliases.First()} {manifestsPath ?? "null"}'. Path does not exist...{Environment.NewLine}";
    }
    string? fluxKustomizationPath = commandResult.GetValueForOption(_fluxKustomizationPathOption);
    fluxKustomizationPath = string.IsNullOrEmpty(fluxKustomizationPath) ? $"clusters/{name}" : fluxKustomizationPath;
    string? realFluxKustomizationPath = Path.Join(manifestsPath, fluxKustomizationPath);
    if (!ValidatePathExists(realFluxKustomizationPath))
    {
      commandResult.ErrorMessage += $"Invalid option '{_fluxKustomizationPathOption.Aliases.First()} {fluxKustomizationPath ?? "null"}'. {realFluxKustomizationPath} does not exist...";
    }
  }

  static bool ValidatePathExists(string? path) =>
    !string.IsNullOrEmpty(path) &&
    (Directory.Exists(path) ||
    File.Exists(path));
}
