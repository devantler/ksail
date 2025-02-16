using System.CommandLine;

namespace KSail.Commands.Secrets.Arguments;

class PublicKeyArgument(string description) : Argument<string>(
  "public-key",
  description
)
{
}
