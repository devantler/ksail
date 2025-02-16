using System.CommandLine;

class KeyArgument(string description) : Argument<string>(
  "key",
  description
)
{
}
