using System.CommandLine;
using KSail.Commands.Down.Handlers;
using KSail.Commands.Down.Options;
using KSail.Deserializer;
using KSail.Options;

namespace KSail.Commands.Down;

sealed class KSailDownCommand : Command
{
  readonly KSailClusterDeserializer _deserializer = new();
  readonly NameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly DistributionOption _distributionOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly RegistriesOption _registriesOption = new();
  internal KSailDownCommand() : base("down", "Destroy a cluster")
  {
    AddOption(_nameOption);
    AddOption(_distributionOption);
    AddOption(_registriesOption);

    this.SetHandler(async (context) =>
    {
      var config = await _deserializer.LocateAndDeserializeAsync().ConfigureAwait(false);
      await config.SetConfigValueAsync("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption)).ConfigureAwait(false);
      await config.SetConfigValueAsync("Spec.Distribution", context.ParseResult.GetValueForOption(_distributionOption)).ConfigureAwait(false);
      await config.SetConfigValueAsync("Spec.DownOptions.Registries", context.ParseResult.GetValueForOption(_registriesOption)).ConfigureAwait(false);

      var handler = new KSailDownCommandHandler(config);
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
