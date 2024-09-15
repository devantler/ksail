
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataValidatingAdmissionPolicyBindingCommand : Command
{
  public KSailGenNativeMetadataValidatingAdmissionPolicyBindingCommand() : base("validating-admission-policy-binding", "Generate a 'admissionregistration.k8s.io/v1/ValidatingAdmissionPolicyBinding' resource.")
  {
  }
}
