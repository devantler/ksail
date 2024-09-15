
using System.CommandLine;

namespace KSail.Commands.Gen.Commands.Native.Metadata;

class KSailGenNativeMetadataValidatingAdmissionPolicyCommand : Command
{
  public KSailGenNativeMetadataValidatingAdmissionPolicyCommand() : base("validating-admission-policy", "Generate a 'admissionregistration.k8s.io/v1/ValidatingAdmissionPolicy' resource.")
  {
  }
}
