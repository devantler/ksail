
using System.CommandLine;
using KSail.Commands.Gen.Handlers.Native.Metadata;
using KSail.Commands.Gen.Options;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataCustomResourceDefinitionCommand : Command
{
  readonly FileOutputOption _outputOption = new("./custom-resource-definition.yaml");
  readonly KSailGenNativeMetadataCustomResourceDefinitionCommandHandler _handler = new();
  public KSailGenNativeMetadataCustomResourceDefinitionCommand() : base("custom-resource-definition", "Generate a 'apiextensions.k8s.io/v1/CustomResourceDefinition' resource.")
  {
    AddOption(_outputOption);
    this.SetHandler(async (context) =>
      {
        string outputPath = context.ParseResult.GetValueForOption(_outputOption) ?? throw new ArgumentNullException(nameof(_outputOption));
        await _handler.HandleAsync(outputPath, context.GetCancellationToken()).ConfigureAwait(false);
      }
    );
  }
}
