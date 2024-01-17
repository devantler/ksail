using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Up.Handlers;
using KSail.Commands.Up.Options;
using KSail.Models;
using KSail.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KSail.Commands.Up;

internal sealed class KSailUpCommand : Command
{
  private readonly NameArgument nameArgument = new() { Arity = ArgumentArity.ZeroOrOne };
  private readonly ConfigOption configOption = new() { IsRequired = true };
  private readonly ManifestsOption manifestsOption = new();
  private readonly KustomizationsOption kustomizationsOption = new();
  private readonly TimeoutOption timeoutOption = new();
  private readonly NoSOPSOption noSOPSOption = new();
  private readonly NoGitOpsOption noGitOpsOption = new();
  private static readonly IDeserializer yamlDeserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .IgnoreUnmatchedProperties()
    .Build();
  internal KSailUpCommand() : base("up", "Create a K8s cluster")
  {
    AddArgument(nameArgument);
    AddOption(configOption);
    AddOption(manifestsOption);
    AddOption(kustomizationsOption);
    AddOption(timeoutOption);
    AddOption(noSOPSOption);
    AddOption(noGitOpsOption);

    this.SetHandler(async (name, configPath, manifestsPath, kustomizationsPath, timeout, noSOPS, noGitOps) =>
    {
      var config = yamlDeserializer.Deserialize<K3dConfig>(File.ReadAllText(configPath));
      if (string.IsNullOrEmpty(name))
      {
        name = config?.Metadata.Name ?? name;
      }
      await KSailUpCommandHandler.HandleAsync(name, configPath);
      if (!noGitOps)
      {
        await KSailUpGitOpsCommandHandler.HandleAsync(name, manifestsPath, kustomizationsPath, timeout, noSOPS);
      }
    }, nameArgument, configOption, manifestsOption, kustomizationsOption, timeoutOption, noSOPSOption, noGitOpsOption);
  }
}
