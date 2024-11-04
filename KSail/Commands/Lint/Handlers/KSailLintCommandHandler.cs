using Devantler.KubernetesValidator.ClientSide.Schemas;
using Devantler.KubernetesValidator.ClientSide.YamlSyntax;
using KSail.Models;

namespace KSail.Commands.Lint.Handlers;

class KSailLintCommandHandler()
{
  readonly YamlSyntaxValidator _yamlSyntaxValidator = new();
  readonly SchemaValidator _schemaValidator = new();

  internal async Task<bool> HandleAsync(KSailCluster config, CancellationToken cancellationToken)
  {
    try
    {
      if (!Directory.Exists(config.Spec.ManifestsDirectory) || Directory.GetFiles(config.Spec.ManifestsDirectory, "*.yaml", SearchOption.AllDirectories).Length == 0)
      {
        Console.WriteLine($"✔ Skipping, as '{config.Spec.ManifestsDirectory}' directory does not exist or is empty");
        return true;
      }

      Console.WriteLine("⏵ Validating YAML syntax");
      bool yamlIsValid = await _yamlSyntaxValidator.ValidateAsync(config.Spec.ManifestsDirectory, cancellationToken).ConfigureAwait(false);

      Console.WriteLine("⏵ Validating Kubernetes schemas");
      bool schemasAreValid = await _schemaValidator.ValidateAsync(config.Spec.ManifestsDirectory, cancellationToken).ConfigureAwait(false);
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
