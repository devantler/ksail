using System.CommandLine;
using KSail.Commands.Down;

namespace KSail.Tests.Integration.TestUtils;

static class KSailTestUtils
{
  internal static async Task CleanupAsync()
  {
    var ksailDownCommand = new KSailDownCommand();
    _ = await ksailDownCommand.InvokeAsync("ksail --delete-pull-through-registries");
    if (Directory.Exists("k8s"))
    {
      Directory.Delete("k8s", true);
    }
    if (File.Exists("ksail-k3d-config.yaml"))
    {
      File.Delete("ksail-k3d-config.yaml");
    }
    if (File.Exists(".sops.yaml"))
    {
      File.Delete(".sops.yaml");
    }
  }
}
