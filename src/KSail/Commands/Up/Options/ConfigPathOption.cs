using System.CommandLine;

namespace KSail.Commands.Up.Options;

/// <summary>
/// The <c>--config</c> option to specify the path to the cluster configuration file.
/// </summary>
public sealed class ConfigPathOption() : Option<string>(
  ["-c", "--config"],
  "path to the cluster configuration file"
);
