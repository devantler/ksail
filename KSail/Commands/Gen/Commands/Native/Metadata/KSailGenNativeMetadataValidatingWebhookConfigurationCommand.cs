
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataValidatingWebhookConfigurationCommand : Command
{
  public KSailGenNativeMetadataValidatingWebhookConfigurationCommand() : base("validating-webhook-configuration", "Generate a 'admissionregistration.k8s.io/v1/ValidatingWebhookConfiguration' resource.")
  {
  }
}
