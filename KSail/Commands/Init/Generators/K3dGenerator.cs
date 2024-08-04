using KSail.Models.K3d;
using Devantler.TemplateEngine;

namespace KSail.Commands.Init.Generators;

class K3dGenerator
{
  readonly Generator _generator = new(new TemplateEngine());

  internal async Task GenerateK3dConfigAsync(string filePath, string clusterName)
  {
    if (!File.Exists(filePath))
    {
      Console.WriteLine($"✚ Generating K3d Config '{filePath}'");
      await _generator.GenerateAsync(
        filePath,
        $"{AppDomain.CurrentDomain.BaseDirectory}assets/templates/k3d/k3d-config.sbn",
        new K3dConfig { Name = clusterName }
      ).ConfigureAwait(false);
    }
    else
    {
      Console.WriteLine($"✓ K3d Config '{filePath}' already exists");
    }
  }
}
