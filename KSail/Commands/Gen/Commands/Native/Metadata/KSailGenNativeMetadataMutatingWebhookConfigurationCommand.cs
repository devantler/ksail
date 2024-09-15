
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataMutatingWebhookConfigurationCommand : Command
{
  public KSailGenNativeMetadataMutatingWebhookConfigurationCommand() : base("mutating-webhook-configuration", "Generate a 'admissionregistration.k8s.io/v1/MutatingWebhookConfiguration' resource.")
  {
  }
}
