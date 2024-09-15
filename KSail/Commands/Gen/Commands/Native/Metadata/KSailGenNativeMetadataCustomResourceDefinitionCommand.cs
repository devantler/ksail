
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataCustomResourceDefinitionCommand : Command
{
  public KSailGenNativeMetadataCustomResourceDefinitionCommand() : base("custom-resource-definition", "Generate a 'apiextensions.k8s.io/v1/CustomResourceDefinition' resource.")
  {
  }
}
