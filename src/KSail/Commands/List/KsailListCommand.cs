using System.CommandLine;
using KSail.Commands.List.Handlers;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.List;

sealed class KSailListCommand : Command
{
  internal KSailListCommand() : base("list", "List running clusters")
  {
    this.SetHandler(async (context) =>
    {
      var kubernetesDistributionProvisioner = new K3dProvisioner();
      var token = context.GetCancellationToken();
      var handler = new KSailListCommandHandler(kubernetesDistributionProvisioner);
      try
      {
        _ = await handler.HandleAsync(token);
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
