using System.Runtime.Serialization;

namespace KSail.Models.Commands.Init;

/// <summary>
/// The template to use for initializing a KSail cluster.
/// </summary>
public enum KSailInitTemplate
{
  /// <summary>
  /// The simple template for initializing a KSail cluster with a minimal Kustomize setup.
  /// </summary>
  [EnumMember(Value = "simple")]
  Simple
}
