using Devantler.KubernetesValidator.ClientSide.Schemas;
using Devantler.KubernetesValidator.ClientSide.YamlSyntax;
using KSail.Models;
using KSail.Utils;

namespace KSail.Commands.Lint.Handlers;

class KSailLintCommandHandler()
{
  readonly YamlSyntaxValidator _yamlSyntaxValidator = new();
  readonly SchemaValidator _schemaValidator = new();

  internal async Task<bool> HandleAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    try
    {
      if (!Directory.Exists(config.Spec.Project.ManifestsDirectory) || Directory.GetFiles(config.Spec.Project.ManifestsDirectory, "*.yaml", SearchOption.AllDirectories).Length == 0)
      {
        Console.WriteLine($"✔ skipping, as '{config.Spec.Project.ManifestsDirectory}' directory does not exist or is empty");
        return true;
      }

      Console.WriteLine("► validating yaml syntax");
      bool yamlIsValid = await _yamlSyntaxValidator.ValidateAsync(config.Spec.Project.ManifestsDirectory, cancellationToken).ConfigureAwait(false);
      Console.WriteLine("✔ yaml syntax is valid");

      Console.WriteLine("► validating schemas");
      bool schemasAreValid = await _schemaValidator.ValidateAsync(config.Spec.Project.ManifestsDirectory, cancellationToken).ConfigureAwait(false);
      Console.WriteLine("✔ schemas are valid");
      return yamlIsValid && schemasAreValid;
    }
    catch (YamlSyntaxValidatorException ex)
    {
      ExceptionHandler.HandleException(ex);
      return false;
    }
    catch (SchemaValidatorException ex)
    {
      ExceptionHandler.HandleException(ex);
      return false;
    }
  }
}
