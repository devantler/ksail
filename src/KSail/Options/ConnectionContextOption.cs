using System.CommandLine;

namespace KSail.Options;

/// <summary>
/// The kube context to use for the connection.
/// </summary>
public class ConnectionContextOption() : Option<string>(
  ["-c", "--context"],
  "The kubernetes context to use"
)
{
}
