using System.CommandLine;
using KSail.Commands.Down.Handlers;
using KSail.Commands.Down.Options;
using KSail.Options;

namespace KSail.Commands.Down;

sealed class KSailDownCommand : Command
{
  readonly NameOption _nameOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly DistributionOption _distributionOption = new() { Arity = ArgumentArity.ZeroOrOne };
  readonly RegistriesOption _registriesOption = new() { Arity = ArgumentArity.ZeroOrOne };
  internal KSailDownCommand() : base("down", "Destroy a cluster")
  {
    AddOption(_nameOption);
    AddOption(_distributionOption);
    AddOption(_registriesOption);

    this.SetHandler(async (context) =>
    {
      var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
      config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));
      config.UpdateConfig("Spec.Distribution", context.ParseResult.GetValueForOption(_distributionOption));
      config.UpdateConfig("Spec.DownOptions.Registries", context.ParseResult.GetValueForOption(_registriesOption));

      var handler = new KSailDownCommandHandler(config);
      try
      {
        Console.WriteLine($"🔥 Destroying cluster '{config.Spec.Distribution}-{config.Metadata.Name}'");
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false);
        Console.WriteLine("");
      }
      catch (OperationCanceledException)
      {
        Console.WriteLine("✕ Operation was canceled by the user.");
        context.ExitCode = 1;
      }
    });
  }
}
