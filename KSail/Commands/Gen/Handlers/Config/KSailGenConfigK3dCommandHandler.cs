using Devantler.KubernetesGenerator.K3d;
using Devantler.KubernetesGenerator.K3d.Models;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Config;

class KSailGenConfigK3dCommandHandler
{
  readonly K3dConfigGenerator _generator = new();
  public async Task HandleAsync(string outputPath, CancellationToken cancellationToken = default)
  {
    var k3dConfig = new K3dConfig
    {
      Metadata = new V1ObjectMeta
      {
        Name = "my-k3d-config"
      }
    };

    await _generator.GenerateAsync(k3dConfig, outputPath, cancellationToken: cancellationToken).ConfigureAwait(false);
    Console.WriteLine($"✚ Generating {outputPath}");
  }
}
