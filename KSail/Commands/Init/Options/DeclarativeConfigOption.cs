using System.CommandLine;

namespace KSail.Commands.Init.Options;

class DeclarativeConfigOption() : Option<bool>(
  ["-kc", "--ksail-config"],
  () => false,
  "Generate a ksail-config.yaml file, to configure the KSail CLI declaratively."
)
{
}
