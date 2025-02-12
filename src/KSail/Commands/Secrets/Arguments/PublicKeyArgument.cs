using System.CommandLine;

class PublicKeyArgument() : Argument<string>(
  "public-key",
  "The public key to delete."
)
{
}
