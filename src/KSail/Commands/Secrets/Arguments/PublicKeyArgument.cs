using System.CommandLine;

namespace KSail.Commands.Secrets.Arguments;

class PublicKeyArgument() : Argument<string>(
  "public-key",
  "The public key to delete."
)
{
}
