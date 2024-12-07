using System.CommandLine;
using Devantler.K3dCLI;
using Devantler.KindCLI;
using KSail.Commands.Down.Handlers;
using KSail.Commands.Down.Options;
using KSail.Options;
using KSail.Utils;
using YamlDotNet.Core;

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
      try
      {
        var config = await KSailClusterConfigLoader.LoadAsync().ConfigureAwait(false);
        config.UpdateConfig("Metadata.Name", context.ParseResult.GetValueForOption(_nameOption));
        config.UpdateConfig("Spec.Project.Distribution", context.ParseResult.GetValueForOption(_distributionOption));
        config.UpdateConfig("Spec.CLI.DownOptions.Registries", context.ParseResult.GetValueForOption(_registriesOption));

        var handler = new KSailDownCommandHandler(config);
        Console.WriteLine($"🔥 Destroying cluster '{config.Spec.Project.Distribution.ToString().ToLower(System.Globalization.CultureInfo.CurrentCulture)}-{config.Metadata.Name}'");
        context.ExitCode = await handler.HandleAsync(context.GetCancellationToken()).ConfigureAwait(false) ? 0 : 1;
        Console.WriteLine();
      }
      catch (YamlException ex)
      {
        ExceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (KindException ex)
      {
        ExceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (K3dException ex)
      {
        ExceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionHandler.HandleException(ex);
        context.ExitCode = 1;
      }
    });
  }
}
