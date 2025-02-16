using System.CommandLine;

class PublicKeyArgument(string description) : Argument<string>(
  "public-key",
  description
)
{
}
