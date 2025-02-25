using System.ComponentModel;
using YamlDotNet.Serialization;

namespace KSail.Models.SecretManager;

/// <summary>
/// Options for the Secret Manager.
/// </summary>
public class KSailSecretManager
{
  /// <summary>
  /// Options for the SOPS secret manager.
  /// </summary>
  [Description("The options for the SOPS secret manager.")]
  [YamlMember(Alias = "sops")]
  public KSailSecretManagerSOPS SOPS { get; set; } = new();
}
