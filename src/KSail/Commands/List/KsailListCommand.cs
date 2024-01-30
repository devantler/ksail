using System.CommandLine;
using KSail.Commands.List.Handlers;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.List;

sealed class KSailListCommand : Command
{
  internal KSailListCommand(CancellationToken token) : base("list", "List running clusters")
  {
    this.SetHandler(async () =>
    {
      var kubernetesDistributionProvisioner = new K3dProvisioner();

      var handler = new KSailListCommandHandler(kubernetesDistributionProvisioner);
      _ = await handler.HandleAsync(token);
    });
  }
}
