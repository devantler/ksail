using System.CommandLine;
using Devantler.K9sCLI;

namespace KSail.Options;

/// <summary>
/// The editor to use for the project.
/// </summary>
public class ProjectEditorOption() : Option<Editor?>(
  ["--editor", "-e"],
  "Editor to use"
);
