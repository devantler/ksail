using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class ConfigPathOption() : Option<string>(
 ["--config-path", "-cp"],
  "path to the cluster configuration file"
);
