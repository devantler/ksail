using System.CommandLine;
using KSail.Commands.Down.Handlers;

namespace KSail.Commands.Down;

/// <summary>
/// The 'ksail down k3d' command responsible for destroying K3d clusters.
/// </summary>
public class KSailDownK3dCommand : Command
{
  /// <summary>
  /// Initializes a new instance of the <see cref="KSailDownK3dCommand"/> class.
  /// </summary>
  /// <param name="nameOption">The -n, --name option.</param>
  public KSailDownK3dCommand(
    Option<string> nameOption
  ) : base("k3d", "destroy a K3d cluster ")
  {
    AddOption(nameOption);

    this.SetHandler(KSailDownK3dCommandHandler.Handle, nameOption);
  }
}
