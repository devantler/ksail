using System.CommandLine;
using KSail.Arguments;
using KSail.Commands.Init.Handlers;
using KSail.Commands.Init.Options;

namespace KSail.Commands.Init;

sealed class KSailInitCommand : Command
{
  readonly NameArgument _nameArgument = new();
  readonly DistributionOption _distributionOption = new();
  readonly OutputOption _outputOption = new();
  readonly TemplateOption _templateOption = new();
  public KSailInitCommand() : base("init", "Initialize a cluster")
  {
    AddArgument(_nameArgument);
    AddOption(_distributionOption);
    AddOption(_templateOption);
    AddOption(_outputOption);

    this.SetHandler(async (context) =>
    {
      string name = context.ParseResult.GetValueForArgument(_nameArgument);
      var distribution = context.ParseResult.GetValueForOption(_distributionOption);
      var template = context.ParseResult.GetValueForOption(_templateOption);
      string outputPath = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
      try
      {
        var handler = new KSailInitCommandHandler(name, distribution, outputPath, template);
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
