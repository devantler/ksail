using Devantler.KubernetesGenerator.K3d;
using Devantler.KubernetesGenerator.K3d.Models;
using Devantler.KubernetesGenerator.KSail.Models;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Config;

class KSailGenConfigK3dCommandHandler
{
  readonly KSailCluster _config;
  readonly K3dConfigGenerator _generator = new();

  internal KSailGenConfigK3dCommandHandler(KSailCluster config)
  {
    _config = config;
  }

  public async Task HandleAsync(CancellationToken cancellationToken = default)
  {
    var k3dConfig = new K3dConfig
    {
      Metadata = new V1ObjectMeta
      {
        Name = "my-k3d-config"
      }
    };

    await _generator.GenerateAsync(k3dConfig, _config.Spec?.ManifestsDirectory!, cancellationToken: cancellationToken).ConfigureAwait(false);
    Console.WriteLine($"✚ Generating {outputPath}");
  }
}
