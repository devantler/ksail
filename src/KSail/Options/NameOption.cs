using System.CommandLine;

namespace KSail.Options;

public class NameOption(string description) : Option<string>(["-n", "--name"], description)
{
}
