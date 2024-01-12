using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class ConfigOption() : Option<string>(
 ["--config", "-c"],
  "Path to the cluster configuration file"
);
