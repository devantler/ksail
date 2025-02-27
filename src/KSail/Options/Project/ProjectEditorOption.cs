using System.CommandLine;
using KSail.Models;
using KSail.Models.Project.Enums;

namespace KSail.Options.Project;


internal class ProjectEditorOption(KSailCluster config) : Option<KSailEditorType?>(
  ["--editor", "-e"],
  $"Editor to use. [default: {config.Spec.Project.Editor}]"
);
