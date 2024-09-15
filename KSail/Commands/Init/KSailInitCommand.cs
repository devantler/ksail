using System.CommandLine;
using Devantler.KubernetesGenerator.KSail.Models;
using KSail.Arguments;
using KSail.Commands.Init.Handlers;
using KSail.Commands.Init.Options;
using KSail.Deserializer;

namespace KSail.Commands.Init;

sealed class KSailInitCommand : Command
{
  readonly ClusterNameArgument _nameArgument = new();
  readonly DistributionOption _distributionOption = new();
  readonly OutputOption _outputOption = new();
  readonly TemplateOption _templateOption = new();
  readonly KSailClusterConfigDeserializer _kSailClusterConfigDeserializer = new();
  public KSailInitCommand() : base("init", "Initialize a cluster")
  {
    AddArgument(_nameArgument);
    AddOption(_distributionOption);
    AddOption(_templateOption);
    AddOption(_outputOption);

    this.SetHandler(async (context) =>
    {
      var config = await _kSailClusterConfigDeserializer.LocateAndDeserializeAsync().ConfigureAwait(false);
      config.Metadata.Name = context.ParseResult.GetValueForArgument(_nameArgument);
      config.Spec = new KSailClusterSpec
      {
        Distribution = context.ParseResult.GetValueForOption(_distributionOption)
      };
      var template = context.ParseResult.GetValueForOption(_templateOption);
      string outputPath = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
      try
      {
        var distribution = config.Spec.Distribution ?? throw new ArgumentNullException(nameof(config.Spec.Distribution));
        var handler = new KSailInitCommandHandler(config.Metadata.Name, distribution, outputPath, template);
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
