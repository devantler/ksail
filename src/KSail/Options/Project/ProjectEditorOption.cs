using System.CommandLine;
using KSail.Models;
using KSail.Models.Project.Enums;

namespace KSail.Options.Project;

/// <summary>
/// The editor to use for the project.
/// </summary>
public class ProjectEditorOption(KSailCluster config) : Option<KSailEditorType?>(
  ["--editor", "-e"],
  $"Editor to use. [default: {config.Spec.Project.Editor}]"
);
