using System.CommandLine;

namespace KSail.Options;

/// <summary>
/// The 'name' option responsible for specifying the name of a resource with -n or --name.
/// </summary>
/// <param name="description">The description to display for the 'name' option.</param>
public class NameOption(string description) : Option<string>(["-n", "--name"], description)
{
}
