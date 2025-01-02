using System.CommandLine;

namespace KSail.Commands.Up.Options;

sealed class DestroyOption() : Option<bool?>(
 ["--destroy"],
  "Destroy any existing cluster before provisioning"
);
