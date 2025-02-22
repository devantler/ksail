using System.CommandLine;

namespace KSail.Options;

/// <summary>
/// The timeout for the connection.
/// </summary>
public class ConnectionTimeoutOption() : Option<string>(
  ["-t", "--timeout"],
  "The time to wait for each kustomization to become ready."
)
{
}
