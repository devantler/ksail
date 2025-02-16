using System.CommandLine;

namespace KSail.Commands.Secrets.Arguments;

class PathArgument(string description) : Argument<string>(
  "path",
  description
)
{
}
