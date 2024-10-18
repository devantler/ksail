using Devantler.KubernetesGenerator.K3d;
using Devantler.KubernetesGenerator.K3d.Models;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Config;

class KSailGenConfigK3dCommandHandler(string outputFile)
{
  readonly string _outputFile = outputFile;
  readonly K3dConfigGenerator _generator = new();

  public async Task<int> HandleAsync(CancellationToken cancellationToken = default)
  {
    var k3dConfig = new K3dConfig
    {
      Metadata = new V1ObjectMeta
      {
        Name = "my-k3d-config"
      }
    };

    Console.WriteLine($"✚ Generating {_outputFile}");
    await _generator.GenerateAsync(k3dConfig, _outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
