using System.ComponentModel;
using YamlDotNet.Serialization;

namespace KSail.Models.SecretManager;


public class KSailSecretManager
{

  [Description("The options for the SOPS secret manager.")]
  [YamlMember(Alias = "sops")]
  public KSailSecretManagerSOPS SOPS { get; set; } = new();
}
