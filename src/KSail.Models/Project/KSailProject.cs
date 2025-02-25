using System.ComponentModel;
using KSail.Models.Project.Enums;
using YamlDotNet.Serialization;

namespace KSail.Models.Project;

/// <summary>
/// Options for the KSail project.
/// </summary>
public class KSailProject
{
  /// <summary>
  /// The path to the ksail configuration file.
  /// </summary>
  [Description("The path to the ksail configuration file.")]
  public string ConfigPath { get; set; } = "ksail-config.yaml";

  /// <summary>
  /// The path to the distribution configuration file.
  /// </summary>
  [Description("The path to the distribution configuration file.")]
  public string DistributionConfigPath { get; set; } = "kind-config.yaml";

  /// <summary>
  /// The template used for the project.
  /// </summary>
  [Description("The template used for the project.")]
  public KSailTemplateType Template { get; set; } = KSailTemplateType.Kustomize;

  /// <summary>
  /// The container engine to use.
  /// </summary>
  [Description("The engine to use for running the KSail cluster.")]
  public KSailEngineType Engine { get; set; } = KSailEngineType.Docker;

  /// <summary>
  /// The Kubernetes distribution to use.
  /// </summary>
  [Description("The Kubernetes distribution to use.")]
  public KSailKubernetesDistributionType Distribution { get; set; } = KSailKubernetesDistributionType.Native;


  /// <summary>
  /// The Deployment tool to use.
  /// </summary>
  [Description("The Deployment tool to use.")]
  public KSailDeploymentToolType DeploymentTool { get; set; } = KSailDeploymentToolType.Flux;

  /// <summary>
  /// The secret manager to use.
  /// </summary>
  [Description("The secret manager to use.")]
  public KSailSecretManagerType SecretManager { get; set; } = KSailSecretManagerType.None;

  /// <summary>
  /// The CNI to use.
  /// </summary>
  [Description("The CNI to use.")]
  [YamlMember(Alias = "cni")]
  public KSailCNIType CNI { get; set; } = KSailCNIType.Default;

  /// <summary>
  /// The editor to use for viewing files while debugging.
  /// </summary>
  [Description("The editor to use for viewing files while debugging.")]
  public KSailEditorType Editor { get; set; } = KSailEditorType.Nano;

  /// <summary>
  /// Whether to set up mirror registries for the project.
  /// </summary>
  [Description("Whether to set up mirror registries for the project.")]
  public bool MirrorRegistries { get; set; } = true;
}
