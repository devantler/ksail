using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class ConfigPathOption() : Option<string>(
 ["--config-path", "-cp"],
  "Path to the cluster configuration file"
);
