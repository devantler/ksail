using KSail.Models;
using KSail.Options.CNI;
using KSail.Options.Connection;
using KSail.Options.DeploymentTool;
using KSail.Options.Distribution;
using KSail.Options.Generator;
using KSail.Options.IngressController;
using KSail.Options.LocalRegistry;
using KSail.Options.Metadata;
using KSail.Options.MirrorRegistries;
using KSail.Options.Project;
using KSail.Options.SecretManager;
using KSail.Options.Template;
using KSail.Options.Validation;
using KSail.Options.WaypointController;

namespace KSail.Options;

/// <summary>
/// Options for the KSail CLI.
/// </summary>
public static class CLIOptions
{
  /// <summary>
  /// The KSail cluster configuration.
  /// </summary>
  static readonly KSailCluster _config = new();

  /// <summary>
  /// CNI options.
  /// </summary>
  public static readonly CNIOptions CNI = new();

  /// <summary>
  /// Connection options.
  /// </summary>
  public static readonly ConnectionOptions Connection = new(_config);

  /// <summary>
  /// Deployment tool options.
  /// </summary>
  public static readonly DeploymentToolOptions DeploymentTool = new(_config);

  /// <summary>
  /// Distribution options.
  /// </summary>
  public static readonly DistributionOptions Distribution = new(_config);

  /// <summary>
  /// Generator options.
  /// </summary>
  public static readonly GeneratorOptions Generator = new();

  /// <summary>
  /// Ingress controller options.
  /// </summary>
  public static readonly IngressControllerOptions IngressController = new();

  /// <summary>
  /// Local registry options.
  /// </summary>
  public static readonly LocalRegistryOptions LocalRegistry = new();

  /// <summary>
  /// Metadata options.
  /// </summary>
  public static readonly MetadataOptions Metadata = new(_config);

  /// <summary>
  /// Mirror registry options.
  /// </summary>
  public static readonly MirrorRegistriesOptions MirrorRegistries = new();

  /// <summary>
  /// Project options.
  /// </summary>
  public static readonly ProjectOptions Project = new(_config);

  /// <summary>
  /// Secret manager options.
  /// </summary>
  public static readonly SecretManagerOptions SecretManager = new(_config);

  /// <summary>
  /// Template options.
  /// </summary>
  public static readonly TemplateOptions Template = new(_config);

  /// <summary>
  /// Validation options.
  /// </summary>
  public static readonly ValidationOptions Validation = new(_config);

  /// <summary>
  /// Waypoint controller options.
  /// </summary>
  public static readonly WaypointControllerOptions WaypointController = new();
}
