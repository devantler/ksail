using System.CommandLine;

namespace KSail.Options;

sealed class NameOption(string description) : Option<string>(["-n", "--name"], description)
{
}
