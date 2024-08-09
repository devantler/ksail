using System.CommandLine;
using KSail.Commands.List.Handlers;
using KSail.Provisioners.KubernetesDistribution;

namespace KSail.Commands.List;

sealed class KSailListCommand : Command
{
  internal KSailListCommand() : base("list", "List active clusters")
  {
    this.SetHandler(async (context) =>
    {
      var kubernetesDistributionProvisioner = new K3dProvisioner();
      var token = context.GetCancellationToken();
      var handler = new KSailListCommandHandler(kubernetesDistributionProvisioner);
      try
      {
        var (exitCode, _) = await handler.HandleAsync(token).ConfigureAwait(false);
        context.ExitCode = exitCode;
      }
      catch (OperationCanceledException)
      {
        context.ExitCode = 1;
      }
    });
  }
}
