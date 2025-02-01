using Devantler.KubernetesValidator.ClientSide.Schemas;
using Devantler.KubernetesValidator.ClientSide.YamlSyntax;
using KSail.Models;

namespace KSail.Commands.Lint.Handlers;

class KSailLintCommandHandler()
{
  readonly YamlSyntaxValidator _yamlSyntaxValidator = new();
  readonly SchemaValidator _schemaValidator = new();

  internal async Task<bool> HandleAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    string kubernetesDirectory = Path.Combine(config.Spec.Project.WorkingDirectory, "k8s");
    // if the k8s directory does not exist or is empty, use the working directory instead
    if (!Directory.Exists(kubernetesDirectory) || Directory.GetFiles(kubernetesDirectory, "*.yaml", SearchOption.AllDirectories).Length == 0)
    {
      Console.WriteLine($"► no manifest files found in '{kubernetesDirectory}', using '{config.Spec.Project.WorkingDirectory}' instead");
      kubernetesDirectory = config.Spec.Project.WorkingDirectory;
    }

    if (!Directory.Exists(kubernetesDirectory) || Directory.GetFiles(kubernetesDirectory, "*.yaml", SearchOption.AllDirectories).Length == 0)
    {
      Console.WriteLine($"✔ skipping, as '{kubernetesDirectory}' directory does not exist or is empty");
      return true;
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
