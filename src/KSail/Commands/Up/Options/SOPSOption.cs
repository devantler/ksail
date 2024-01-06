using System.CommandLine;

namespace KSail.Commands.Up.Options;

/// <summary>
/// The 'sops' option responsible for enabling SOPS with -s or --sops.
/// </summary>
public class SOPSOption() : Option<bool>(
  ["-s", "--sops"],
  () => true,
  "enable SOPS"
);
