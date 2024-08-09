using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class ConfigOption() : Option<string>(
 ["--config", "-c"],
 () => "k3d-config.yaml",
  "Path to the cluster configuration file"
);
