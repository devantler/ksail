using System.Runtime.Serialization;

namespace KSail.Models.CLI.Commands.Init;

/// <summary>
/// The template to use for initializing a KSail cluster.
/// </summary>
public enum KSailCLIInitTemplate
{
  /// <summary>
  /// The simple template for initializing a KSail cluster with a minimal Kustomize setup.
  /// </summary>
  [EnumMember(Value = "simple")]
  Simple
}
