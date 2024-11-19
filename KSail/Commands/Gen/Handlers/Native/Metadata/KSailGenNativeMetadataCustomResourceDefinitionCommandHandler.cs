using Devantler.KubernetesGenerator.Native.Metadata;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native.Metadata;

class KSailGenNativeMetadataCustomResourceDefinitionCommandHandler
{
  readonly CustomResourceDefinitionGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1CustomResourceDefinition
    {
      ApiVersion = "apiextensions.k8s.io/v1",
      Kind = "CustomResourceDefinition",
      Metadata = new V1ObjectMeta
      {
        Name = "<plural>.<group>"
      },
      Spec = new V1CustomResourceDefinitionSpec()
      {
        Group = "<group>",
        Versions =
        [
          new V1CustomResourceDefinitionVersion()
          {
            Name = "<version>",
            Served = true,
            Storage = true,
            Schema = new V1CustomResourceValidation()
          }
        ],
        Scope = "Namespaced",
        Names = new V1CustomResourceDefinitionNames()
        {
          Plural = "<plural>",
          Singular = "<singular>",
          Kind = "<kind>",
          ListKind = "<list-kind>",
          ShortNames = ["<short-name>"]
        }
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
