using System.CommandLine;

namespace KSail.Commands.List.Options;

sealed class AllOption() : Option<bool?>(
  ["--all", "-a"],
  "List clusters from all distributions"
);
