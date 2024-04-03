using KSail.Models.KSail;
using KSail.TemplateEngine;

namespace KSail.Commands.Init.Generators;

class KSailGenerator
{
  readonly Generator _generator = new(new TemplateEngine.TemplateEngine());

  internal async Task GenerateKSailConfigAsync(string filePath, string clusterName)
  {
    if (!File.Exists(filePath))
    {
      Console.WriteLine($"✚ Generating KSail Config '{filePath}'");
      await _generator.GenerateAsync(
        filePath,
        $"{AppDomain.CurrentDomain.BaseDirectory}assets/templates/ksail/ksail-config.sbn",
        new KSailConfig(clusterName)
      );
    }
    else
    {
      Console.WriteLine($"✓ KSail Config '{filePath}' already exists");
    }
  }
}
