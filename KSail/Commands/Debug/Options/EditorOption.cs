using System.CommandLine;
using Devantler.K9sCLI;

namespace KSail.Commands.Debug.Options;

sealed class EditorOption() : Option<Editor?>(
  ["--editor", "-e"],
  "Editor to use"
);
