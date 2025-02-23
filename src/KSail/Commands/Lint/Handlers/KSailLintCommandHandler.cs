using Devantler.KubernetesValidator.ClientSide.Schemas;
using Devantler.KubernetesValidator.ClientSide.YamlSyntax;

namespace KSail.Commands.Lint.Handlers;

class KSailLintCommandHandler()
{
  readonly YamlSyntaxValidator _yamlSyntaxValidator = new();
  readonly SchemaValidator _schemaValidator = new();

  internal async Task<bool> HandleAsync(CancellationToken cancellationToken = default)
  {
    string kubernetesDirectory = "k8s";
    if (!Directory.Exists(kubernetesDirectory) || Directory.GetFiles(kubernetesDirectory, "*.yaml", SearchOption.AllDirectories).Length == 0)
    {
      throw new KSailException($"no manifest files found in '{kubernetesDirectory}'.");
    }

    Console.WriteLine("► validating yaml syntax");
    bool yamlIsValid = await _yamlSyntaxValidator.ValidateAsync(kubernetesDirectory, cancellationToken).ConfigureAwait(false);
    Console.WriteLine("✔ yaml syntax is valid");

    Console.WriteLine("► validating schemas");
    bool schemasAreValid = await _schemaValidator.ValidateAsync(kubernetesDirectory, cancellationToken).ConfigureAwait(false);
    Console.WriteLine("✔ schemas are valid");
    return yamlIsValid && schemasAreValid;
  }
}
