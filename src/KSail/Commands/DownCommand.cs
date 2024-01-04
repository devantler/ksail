using System.CommandLine;
using KSail.Enums;
using KSail.Options;
using Spectre.Console;

namespace KSail.Commands;

public class DownCommand : Command
{
  public DownCommand() : base("down", "destroy a K8s cluster in Docker")
  {
    var nameOption = new NameOption("name of the cluster to destroy");
    foreach (var k8sInDockerBackend in Enum.GetValues<K8sInDockerBackend>())
    {
      AddCommand(new DownBackendCommand(k8sInDockerBackend, nameOption));
    }

    this.SetHandler(() =>
    {
      _ = this.InvokeAsync("--help");
    });
  }

  public static Command DownK3dCommand(Option<string> nameOption)
  {
    var downK3dCommand = new Command("k3d", "destroy a k3d cluster");
    downK3dCommand.AddOption(nameOption);

    downK3dCommand.SetHandler((name) =>
    {
      Console.WriteLine($"🔥 Destroying '{name?.ToString()}' cluster...");
    }, nameOption);

    return downK3dCommand;
  }

  public static Command DownTalosCommand(Option<string> nameOption)
  {
    var downTalosCommand = new Command("talos", "destroy a Talos cluster");
    downTalosCommand.AddOption(nameOption);
    return downTalosCommand;
  }
}
