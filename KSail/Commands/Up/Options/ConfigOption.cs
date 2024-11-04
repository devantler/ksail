using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class ConfigOption() : Option<string>(
 ["--config", "-c"],
 () => "kind-config.yaml",
  "Path to the cluster configuration file"
);
