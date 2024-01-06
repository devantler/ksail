using KSail.Provisioners.Cluster;
using KSail.Utils;

namespace KSail.Presentation.Commands.Down.Handlers;

/// <summary>
/// The command handler responsible for handling the <c>ksail down k3d</c> command.
/// </summary>
public static class KSailDownK3dCommandHandler
{
  static readonly K3dProvisioner _provisioner = new();

  /// <summary>
  /// Handles the <c>ksail down k3d</c> command.
  /// </summary>
  /// <param name="name">The name of the cluster to destroy.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  public static async Task Handle(string name)
  {
    name ??= ConsoleUtils.Prompt("Please enter the name of the cluster to destroy");

    Console.WriteLine($"🔥 Destroying K3d cluster '{name}'...");
    await _provisioner.DeprovisionAsync(name);
  }
}
