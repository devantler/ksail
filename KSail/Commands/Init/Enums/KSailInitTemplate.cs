using System.Runtime.Serialization;

namespace KSail.Commands.Init.Enums;

enum KSailInitTemplate
{
  /// <summary>
  /// The default template for initializing a KSail cluster with K3d and Flux.
  /// </summary>
  [EnumMember(Value = "k3d-flux-default")]
  K3dFluxDefault,
}
