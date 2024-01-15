using System.CommandLine.Parsing;
using KSail.Commands.Up.Options;
using KSail.Models;
using KSail.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KSail.Commands.Up.Validators;

static class KSailUpValidator
{
  static readonly IDeserializer yamlDeserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .IgnoreUnmatchedProperties()
    .Build();
  internal static Task ValidateAsync(CommandResult commandResult, NameOption nameOption, ConfigOption configOption)
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

    return Task.CompletedTask;
  }

  static bool ValidatePathExists(string? path) =>
    !string.IsNullOrEmpty(path) &&
    (Directory.Exists(path) ||
    File.Exists(path));
}
