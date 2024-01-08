using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class ConfigPathOption() : Option<string>(
 ["-c", "--config"],
  "path to the cluster configuration file"
);
